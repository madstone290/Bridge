/**
 * 쿠키를 설정한다
 * @param {any} name 쿠키명
 * @param {any} value 쿠키값
 * @param {any} maxAge 만료기한(분단위)
 */
function setCookie(name, value, maxAge) {

    let maxAgeString = '';
    if (maxAge) {
        maxAgeString = `max-age=${maxAge}`;
    }
    document.cookie = `${name}=${value}; ${maxAgeString}; path=/;`;
}

/**
 * 쿠키를 가져온다
 * @param {any} name 쿠키명
 */
function getCookie(name) {
    let nameExpression = name + "=";
    let cookieItems = document.cookie.split(';');
    for (let i = 0; i < cookieItems.length; i++) {
        let item = cookieItems[i];
        while (item.charAt(0) == ' ') {
            item = item.substring(1);
        }
        if (item.indexOf(nameExpression) == 0) {
            return item.substring(nameExpression.length, item.length);
        }
    }
    return null;
}