<?
	include "conndb.php";

	$db_link=db_conn();  //데이터베이스와 연결하는 사용자 정의 함수
	mysqli_select_db($db_link,$DB_SNAME); //내부 database 선택

	$SQL = " select code, title, contents, name, adddate, 'N' as changed from board order by adddate desc ";
	$result = mysqli_query($db_link, $SQL);
	$boardResult = dbresultTojson($result);

	$SQL = " select code, title, contents, name, adddate, 'N' as changed from notice order by adddate desc ";
	$result = mysqli_query($db_link, $SQL);
	$noticeResult = dbresultTojson($result);

	function dbresultTojson($res)
	{
		$ret_arr = array();

		while($row = mysqli_fetch_array($res))
		{
			foreach($row as $key => $value){
				$row_array[$key] = urlencode($value);
			}
			array_push($ret_arr, $row_array);
		}

		return urldecode(json_encode($ret_arr));
	}

	if($_POST["mode"] == "allsave") //일괄저장하기
	{
		//boardName, boardData 를 활용해서 mysql update 시켜야 함
		//echo "정상실행되었음";
		//$_POST["boardName"] : 현재 게시판 종류

		$boardData = json_decode($_POST["boardData"]);
		$deleteLists = json_decode($_POST["deleteLists"]);
		

		$tableName = "";
		if($_POST["boardName"] == "free")
		{
			$tableName = "board"; 
		}
		else if($_POST["boardName"] == "notice")
		{
			$tableName = "notice"; 
		}

		for($i=0; $i < count($deleteLists); $i++)
		{
			$code = $deleteLists[$i]->code;
			if($code) //삭제
			{
				$SQL = "delete from $tableName where code='".$code."'";
				mysqli_query($db_link, $SQL);
			}
		}

		for($i=0; $i < count($boardData); $i++)
		{
			$code = $boardData[$i]->code;
			$title = $boardData[$i]->title;
			$contents = $boardData[$i]->contents;
			$name = $boardData[$i]->name;
			$changed = $boardData[$i]->changed; //변경 여부 (Y, N)

			//새글추가된 경우는 데이터베이스에 insert
			if(!$code)
			{
				$SQL = " insert into ".$tableName."(title, name, adddate) values('".addslashes($title)."', '".addslashes($name)."', now()) ";
				mysqli_query($db_link, $SQL);
			}

			//기존의 글이라면, update
			if($changed == "Y")  //데이터베이스에 update
			{
				$SQL = " update ".$tableName." set title='".addslashes($title)."', name='".addslashes($name)."' where code='".$code."' ";
				mysqli_query($db_link, $SQL);
			}
		}

		echo "OK";
		exit;
	}
?>
<!DOCTYPE html>
<html lang="en">
<head>
	<meta charset="utf-8">
	<title>Vuejs 게시판</title>
	<script src="jquery.min.js"></script>
	<script src="jquery-ui.min.js"></script>
	<script src="https://cdn.jsdelivr.net/npm/vue/dist/vue.js"></script>
   <script src="jquery.bpopup2.min.js"></script>
	<style>
	.clBoardHeader {
		background-color:#aeaeae;
		color:white;
	}
	.clBoardHeader th {
		padding-top:7px;
		padding-bottom:7px;
	}

	.clViewHeader {
		width: 10%;
		float:left;
		padding-left:12px;
		margin-top:5px;
	}

	.clViewContent {
		width: 80%;
		float:left;
		padding-left:7px;
		margin-top:5px;
	}

	</style>
</head>
<body>

	<div style="width:80%; margin:0px auto; margin-top:50px; text-align:right">
		<button id="tableButton1" v-bind:style="{'background-color':bgcolor, 'border':'0px' ,'color':fontcolor}" v-on:click="checkActivate">자유게시판 보기</button>
		<button id="tableButton2" v-bind:style="{'background-color':bgcolor, 'border':'0px' ,'color':fontcolor}" v-on:click="checkActivate">공지사항 보기</button>
	</div>

	<div id="tableBoard" style="width:80%; margin:0px auto; padding-top:20px">
		<div style="margin-bottom:5px">
			<input type=text v-model="searchName" style="width:90%; height:25px"> <!-- 검색 바 -->
			<button v-on:click="insertNewPosting()">새글추가</button>
		</div>
		<table style="width:100%" align="center">
			<thead>
				<tr class="clBoardHeader">
					<th style="width:70px">No.</th>
					<th style="width:120px">이름</th>
					<th>제목</th>
					<th style="width:170px">날짜</th>
					<th style="width:160px">액션</th>
				</tr>
			</thead>
			<tbody>
				<tr v-for="(eachData, index) in boardData" class="clBoardBody" v-if="eachData.title.includes(searchName)">
					<td style="width:70px" align="center">{{index+1}}</td>
					<td style="width:120px"><input type=text v-model="eachData.name" v-on:change="changedData(eachData)" style="width:90%"></td>
					<td><input type=text v-model="eachData.title" v-on:change="changedData(eachData)" style="width:90%"></td>
					<td style="width:170px">{{eachData.adddate}}</td>
					<td style="width:160px" align="center"><button v-on:click="clickTitle(eachData)">내용보기</button> <button v-on:click="deletePosting(eachData, index)">삭제하기</button></td>
				</tr>
			</tbody>
		</table>
	</div>

	<div style="width:80%; padding-top:24px; margin:0px auto; text-align:right">
		<button onclick="javascript:allSave();">일괄 저장하기</button>
	</div>

	<div id="boardView" style="display:none; width:900px; background-color:white; padding-top:10px; padding-bottom:30px">
		<div style="font-size:20px; font-weight:bold; padding-left:10px; margin-bottom:10px">
			글보기
		</div>
		<div style="clear:both">
			<div class="clViewHeader">이름</div>
			<div class="clViewContent">{{boardViewName}}</div>
		</div>
		<div style="clear:both">
			<div class="clViewHeader">제목</div>
			<div class="clViewContent">{{boardViewTitle}}</div>
		</div>
		<div style="clear:both">
			<div class="clViewHeader">내용</div>
			<div class="clViewContent">{{boardViewContents}}</div>
		</div>
		<div style="clear:both">
			<div class="clViewHeader">날짜</div>
			<div class="clViewContent">{{boardViewDate}}</div>
		</div>
	</div>
</body>
</html>
<script>
	var board1Button = null;
	var board2Button = null;
	var boardView = null;
	var app = null;
	$(document).ready(function() {

		var dbdataBoard = <? echo $boardResult; ?>;  //자유게시판 데이터
		var dbdataNotice = <? echo $noticeResult; ?>; //공지사항 데이터
		
		var deleteDataBoard = [];  //자유게시판의 삭제된 글 정보가 담깁니다
		var deleteDataNotice = []; //공지사항의 삭제된 글 정보가 담깁니다

		app = new Vue({
			el: '#tableBoard',
			data: {
				boardData: dbdataBoard,
				deleteLists: deleteDataBoard,
				searchName: ''
			},
			methods: {
				clickTitle: function(boardViewData){
//					alert(boardViewData.name);
					boardView.boardViewName = boardViewData.name;
					boardView.boardViewTitle = boardViewData.title;
					boardView.boardViewContents = boardViewData.contents;
					boardView.boardViewDate = boardViewData.adddate;
					$("#boardView").bPopup();
				},
				changedData: function(boardViewData){
					boardViewData.changed = "Y";
				},
				insertNewPosting: function(){
					//새글쓰기 구현부분
					this.boardData.push({"code":"", "title":"", "contents":"", "name":"", "adddate":"", "changed":"N"});
				},
				deletePosting: function(boardViewData, idx){
					//특정글 삭제하기 구현
					this.boardData.splice(idx, 1);

					//삭제 정보 변수에 삭제하고자 하는 글을 추가
					this.deleteLists.push({"code": boardViewData.code});
				}
			}
		});

	  	var activeBgColor = '#666666';
		var deactiveBgColor = '#dddddd';

		board1Button = new Vue({   //자유게시판 보기 버튼
		  el: '#tableButton1',
		  data: {bgcolor:activeBgColor, fontcolor: 'white', isActive: true},
		  methods: {
			  checkActivate: function() {
				  if(this.isActive === false)
				  {
					  this.bgcolor = activeBgColor;
					  this.isActive = true;
					  board2Button.bgcolor = deactiveBgColor;
					  board2Button.isActive = false;
					  app.boardData = dbdataBoard;
					  app.deleteLists = deleteDataBoard;
				  }
			  }
		  }
		});

		board2Button = new Vue({  //공지사항 보기
		  el: '#tableButton2',
		  data: {bgcolor:deactiveBgColor, fontcolor: 'white', isActive: false},
		  methods: {
			  checkActivate: function() {
				  if(this.isActive === false)
				  {
					  this.bgcolor = activeBgColor;
					  this.isActive = true;
					  board1Button.bgcolor = deactiveBgColor;
					  board1Button.isActive = false;
  					  app.boardData = dbdataNotice;
					  app.deleteLists = deleteDataNotice;
				  }
			  }
		  }
		});

		boardView = new Vue({
			el: '#boardView',
			data: {
				boardViewName: '',
				boardViewTitle: '',
				boardViewContents: '',
				boardViewDate: ''
			}
		});
	});

	function allSave()  //일괄저장하기
	{
		var sendingData = app.boardData;

		var boardName = "";
		if(board1Button.isActive)  //자유게시판
		{
			boardName = "free";
		}
		else //공지사항
		{
			boardName = "notice";
		}

		$.ajax({
		  type: 'POST',  
		  url: "index.php",
		  data: { 'mode':'allsave','boardName': boardName, 'boardData':JSON.stringify(sendingData), 'deleteLists':JSON.stringify(app.deleteLists)},
		  dataType : 'text',
		  cache: false,
		  async: false
		})
		.done(function( result ) {
			  if(result == "OK")
			  {
				  alert("정상 저장되었습니다.");
				  location.href = "index.php";
			  }
	   })
      .fail(function(request,status,error){
			  alert("에러 발생: "+error);
      });
	}
</script>

