export function scrollTo(itemId) {
    let listItem = document.getElementById(itemId);
    let listItemTop = listItem.getBoundingClientRect().top;
    console.log(listItem.getBoundingClientRect());

    let list = document.getElementById("mudlist");
    let listTop = list.getBoundingClientRect().top;
    console.log(list.getBoundingClientRect());

    list.scrollTo(0, list.scrollTop + listItemTop - listTop);
}