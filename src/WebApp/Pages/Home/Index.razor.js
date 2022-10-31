/**
 * 리스트 내의 특정 아이템까지 스크롤한다
 * @param {any} itemId 스크롤할 아이템의 아이디
 */
export function scrollTo(itemId) {
    let listItem = document.getElementById(itemId);
    let listItemTop = listItem.getBoundingClientRect().top;
    console.log(listItem.getBoundingClientRect());

    let list = document.getElementById("mudlist");
    let listTop = list.getBoundingClientRect().top;
    console.log(list.getBoundingClientRect());

    list.scrollTo(0, list.scrollTop + listItemTop - listTop);
}