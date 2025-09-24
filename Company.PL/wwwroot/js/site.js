var searchInp = document.getElementById("EmpSearchInp");
let value = searchInp.value;
searchInp.addEventListener("keyup",
    function () {
       let xhr = new XMLHttpRequest();
        let url = `https://localhost:7215/Employees/Index?searchInp=${searchInp.value}`
        xhr.open("Get", url, true);
        xhr.onreadystatechange = function () {
            if (this.readyState == 4 && this.status == 200) {
                console.log(this.response)
            }
        }
        xhr.send();
       
    }
)