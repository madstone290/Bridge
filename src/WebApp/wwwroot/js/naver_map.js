/*
 * 네이버 맵 api를 제공한다.
 * */

/**
 * 닷넷참조 함수식별자. 지도 클릭시 호출
 * */
const OnClickId = 'OnClick';

/**
 * 닷넷참조 함수식별자. 중심 위치 변경시 호출
 * */
const OnCenterChangedId = 'OnCenterChanged';

/**
 * 닷넷참조 맵. SessionId를 키로 사용한다.
 * */
let _dotNetRefMap = new Map();

/**
 * 네이버맵 맵. SessionId를 키로 사용한다.
 * */
let _mapMap = new Map();

/**
 * 현재 위치 마커 맵. SessionId를 키로 사용한다.
 * */
let _markerMap = new Map();

/**
 * 네이버 맵을 초기화 한다
 * @param {string} sessionId 세션 아이디
 * @param {any} dotNetRef 닷넷 참조객체
 * @param {string} mapId 맵초기화를 위한 div 요소 아이디
 * @param {number} centerX X축 중심위치. 경도
 * @param {number} centerY Y축 중심위치. 위도
 * @param {boolean} showMarker 마커 사용여부
 */
export function init(sessionId, dotNetRef, mapId, centerX, centerY, showMarker) {
    console.log(sessionId);
    _dotNetRefMap.set(sessionId, dotNetRef);

    let mapOptions = {
        zoom: 17
    };
    if (centerX && centerY)
        mapOptions.center = new naver.maps.LatLng(centerY, centerX);

    let map = new naver.maps.Map(mapId, mapOptions);
    _mapMap.set(sessionId, map);

    if (showMarker) {
        let marker = new naver.maps.Marker({
            map: map,
            position: map.getCenter(),
        });
        _markerMap.set(sessionId, marker);
    }

    naver.maps.Event.addListener(map, 'center_changed', function (center) {
        //console.log(center);
        dotNetRef.invokeMethodAsync(OnCenterChangedId, sessionId, center.x, center.y);
    });

    naver.maps.Event.addListener(map, 'zoom_changed', function (zoom) {
        //console.log(zoom);
    });

    // 클릭으로 위치가 변경된 경우
    naver.maps.Event.addListener(map, 'click', function (e) {
        //console.log(e);
        let marker = _markerMap.get(sessionId);
        if (marker)
            marker.setPosition(e.latlng);

        dotNetRef.invokeMethodAsync(OnClickId, sessionId, e.latlng.x, e.latlng.y);
    });
}

/**
 * 맵을 종료한다
 * */
export function close(sessionId) {
    _dotNetRefMap.delete(sessionId);
    _mapMap.delete(sessionId);
    _markerMap.delete(sessionId);
}

export function getMarkerLocation(sessionId) {
    return _markerMap.get(sessionId).getPosition();
}