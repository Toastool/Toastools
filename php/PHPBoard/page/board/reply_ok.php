<?php
	include $_SERVER['DOCUMENT_ROOT']."/db.php";
	date_default_timezone_set('Asia/Seoul');
    $bno = $_GET['idx'];
    $userpw = password_hash($_POST['dat_pw'], PASSWORD_DEFAULT);
    $date = date('Y-m-d H:i:s');

    if($bno && $_POST['dat_user'] && $userpw && $_POST['content']){
        $sql = mq("insert into Toast_boardReply(con_num,name,pw,content,date) values('".$bno."','".$_POST['dat_user']."','".$userpw."','".$_POST['content']."','".$date."')");
        echo "<script>alert('댓글이 작성되었습니다.'); 
        location.href='/page/board/read.php?idx=$bno';</script>";
    }else{
        echo "<script>alert('댓글 작성에 실패했습니다.'); 
        history.back();</script>";
    }
	
?>