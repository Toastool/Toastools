<?php

include $_SERVER['DOCUMENT_ROOT']."/db.php";

//각 변수에 write.php에서 input name값들을 저장한다
$username = $_POST['name'];
$title = $_POST['title'];
$content = $_POST['content'];
$date = date('Y-m-d');
if($username && $title && $content){
    $sql = mq("insert into Toast_board(name,title,content,date) values('".$username."','".$title."','".$content."','".$date."')"); 
    echo $sql;
	echo "<script>
    alert('글쓰기 완료되었습니다.');
    location.href='/';</script>";
}else{
    echo "<script>
    alert('글쓰기에 실패했습니다.');
    history.back();</script>";
}
?>