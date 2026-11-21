document.addEventListener("click",function(e){
    const link=e.target.closest(".loc-ts");
    if(!link){
        return;
    }
    e.preventDefault();
    fetch("/Home/TraVe?id="+link.dataset.id).then(res=>res.text())
    .then(html=>{document.querySelector("#danhSach").innerHTML=html})
})