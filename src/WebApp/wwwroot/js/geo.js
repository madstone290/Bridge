/*
 * Html Geolocation api를 제공한다.
 * */


/**
 * 닷넷참조 함수식별자. 위치 조회 성공시 호출
 * */
const OnSucessId = 'OnSuccess';

/**
 * 닷넷참조 함수식별자. 위치 조회 실패시 호출
 * */
const OnErrorId = 'OnError';

/**
 * 닷넷참조
 * */
let _dotNetRef;

/**
 * 닷넷참조를 설정한다
 * @param {any} dotNetRef 참조객체
 */
export function setDotNetRef(dotNetRef) {
    _dotNetRef = dotNetRef;
}

/**
 * 현재 위치를 확인한다
 * */
export function getLocation() {
    if (navigator.geolocation) {
        navigator.geolocation.getCurrentPosition(successCallback, errorCallback);
    }
}

/**
 * 위치 확인이 성공했을 떄의 콜백
 * @param {any} position 위치
 */
function successCallback(position) {
    if (!_dotNetRef)
        return;
    _dotNetRef.invokeMethodAsync(OnSucessId, position.coords.latitude, position.coords.longitude);
}

/**
 * 위치 확인에서 에러가 발생한 때의 콜백
 * @param {any} error 에러
 */
function errorCallback(error) {
    if (!_dotNetRef)
        return;
    _dotNetRef.invokeMethodAsync(OnErrorId, error.code, error.message);
}