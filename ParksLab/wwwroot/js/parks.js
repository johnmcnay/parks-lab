let search = document.querySelector("#search");
let target = document.querySelector("#target");

search.addEventListener("keyup", () => {

    let query = search.value.trim();
    if (query != "") {
        fetch('/park?filter=' + query)
            .then(response => response.json())
            .then(data => {

                target.innerHTML = "";

                displayParkInfoFromJson(data);

            });
    }

    function displayParkInfoFromJson(data) {

        for (let obj of data) {

            let h = document.createElement("h4");
            h.textContent = obj.parkname;
            target.appendChild(h);

            let p = document.createElement("p");
            p.innerHTML = "<i>Borough: </i>" + obj.borough;
            target.appendChild(p);

            p = document.createElement("p")
            p.innerHTML = "<i>Acres: </i>" + obj.acres;
            target.appendChild(p);

            p = document.createElement("p")
            p.innerHTML = "<i>Description: </i>" + obj.description;
            target.appendChild(p);
        }

    }
});