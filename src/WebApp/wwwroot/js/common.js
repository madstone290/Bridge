/**
 * 아이템 위치까지 스크롤한다.
 * @param {any} parentId 아이템이 속한 부모 엘리멘트의 아이디
 * @param {any} itemId 아이템 엘리멘트의 아이디
 */
export function scroll(parentId, itemId) {
    let parent = document.getElementById(parentId);
    let parentTop = parent.getBoundingClientRect().top;

    let item = document.getElementById(itemId);
    let itemTop = item.getBoundingClientRect().top;

    parent.scrollTo(0, parent.scrollTop + itemTop - parentTop);
}


export function isMobileBrowser() {
    return /android|webos|iphone|ipad|ipod|blackberry|iemobile|opera mini|mobile/i.test(navigator.userAgent);
}