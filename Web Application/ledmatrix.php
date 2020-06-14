<?php
if (isset($_POST))
{
    echo "success";
    $data = file_get_contents('php://input');
    file_put_contents('scripts/ledmatrix.json',$data);
    shell_exec('sudo /scripts/ledmatrix.py 2>&1');
}
else
{
    echo "failure";
}
