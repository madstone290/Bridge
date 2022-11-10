/*
 * 네이버 맵 api를 제공한다.
 * */

/**
 * 닷넷참조 함수식별자. 중심 위치 변경시 호출
 * */
const OnCenterChangedId = 'OnCenterChanged';

/**
 * 닷넷참조 함수식별자. 사용자가 마커를 선택하는 경우 호출
 * */
const OnSelectedMarkerChangedId = 'OnSelectedMarkerChanged';

/**
 * 닷넷참조 함수식별자. 사용자가 컨텍스트 메뉴를 선택하는 경우 호출
 * */
const OnContextMenuClickedId = 'OnContextMenuClicked';

/**
 * 닷넷참조 맵. SessionId를 키로 사용한다.
 * */
let _dotNetRefMap = new Map();

/**
 * 네이버맵 맵. SessionId를 키로 사용한다.
 * */
let _mapMap = new Map();

/**
 * 내 위치 마커 맵. SessionId를 키로 사용한다.
 * */
let _myLocationMarkerMap = new Map();

/**
 * 메뉴 마커 맵. SessionId를 키로 사용한다.
 * */
let _menuMarkerMap = new Map();

/**
 * 마커리스트 맵. SessionId를 키로 사용한다.
 * */
let _markersMap = new Map();

let _selectedMarkerMap = new Map();

/**
 * 마커 아이콘 컬렉션
 * */
const _markerIcons = {
    selectedLocation: {
        content: `
<div style="display:flex; flex-direction:row;">
    <img style="width:24px; height:24px;" src="/img/selected-location-sign.png" />
</div>`,
        origin: new naver.maps.Point(0, 0),
        anchor: new naver.maps.Point(12, 24)
    },
    location: {
        content: `
<div style="display:flex; flex-direction:row;">
    <img style="width:24px; height:24px;" src="/img/location-sign.png" />
</div>`,
        origin: new naver.maps.Point(0, 0),
        anchor: new naver.maps.Point(12, 24)
    },
    menu: {
        content: `
<div style="background-color:white; border:solid 1px #eee;">
    <div id="menu1" style="padding: 4px;" onmouseover="this.style.backgroundColor='#eee'" onmouseleave="this.style.backgroundColor=null">이 위치에서 검색</div>
    <div id="menu2" style="padding: 4px;" onmouseover="this.style.backgroundColor='#eee'" onmouseleave="this.style.backgroundColor=null">이 위치에 장소 추가</div>
</div>`,
        origin: new naver.maps.Point(0, 0),
        anchor: new naver.maps.Point(0, 0)
    }
};

const _markerFunctions = {
    select: function(marker) {
        marker.setIcon(_markerIcons.selectedLocation);
        marker.setZIndex(100);
    },
    deselect: function(marker) {
        marker.setIcon(_markerIcons.location);
        marker.setZIndex(0);
    },
    show: function (marker, map, position) {
        marker.setMap(map);
        marker.setPosition(position);
    },
    hide: function (marker) {
        marker.setMap(null);
    }
}

/**
 * 네이버 맵을 초기화 한다
 * @param {string} sessionId 세션 아이디
 * @param {any} dotNetRef 닷넷 참조객체
 * @param {string} mapId 맵초기화를 위한 div 요소 아이디
 * @param {number} centerX X축 중심위치. 경도
 * @param {number} centerY Y축 중심위치. 위도
 * @param {boolean} showMyLocation 내 위치 표시여부
 */
export function init(sessionId, dotNetRef, mapId, centerX, centerY, showMyLocation) {
    console.log("new session", sessionId);
    _dotNetRefMap.set(sessionId, dotNetRef);

    let mapOptions = {
        zoom: 17
    };
    if (centerX && centerY)
        mapOptions.center = new naver.maps.LatLng(centerY, centerX);
    
    let map = new naver.maps.Map(mapId, mapOptions);
    _mapMap.set(sessionId, map);

    
    let myLocationMarker = new naver.maps.Marker();
    _myLocationMarkerMap.set(sessionId, myLocationMarker);
    

    let menuMarker = new naver.maps.Marker({
        icon: _markerIcons.menu
    });
    naver.maps.Event.addListener(menuMarker, 'click', (e) => {
        console.log(e);
        const menuId = e.domEvent.srcElement.id;
        const x = menuMarker.tag.x;
        const y = menuMarker.tag.y;
        
        dotNetRef.invokeMethodAsync(OnContextMenuClickedId, sessionId, menuId, x, y);

        if (menuId == "menu1") {
            myLocationMarker.setMap(map);
            myLocationMarker.setPosition({ lat: y, lng: x });
        }

        _markerFunctions.hide(menuMarker);
    });

    _menuMarkerMap.set(sessionId, menuMarker);

    naver.maps.Event.addListener(map, 'center_changed', function (center) {
        //console.log(center);
        dotNetRef.invokeMethodAsync(OnCenterChangedId, sessionId, center.x, center.y);
    });

    naver.maps.Event.addListener(map, 'zoom_changed', function (zoom) {
        //console.log(zoom);
    });

    naver.maps.Event.addListener(map, 'mousedown', function (zoom) {
        let menu = _menuMarkerMap.get(sessionId);
        _markerFunctions.hide(menu);
    });

    naver.maps.Event.addListener(map, 'rightclick', function (e) {
        console.log(e);
        let menu = _menuMarkerMap.get(sessionId);
        menu.tag = {
            x: e.latlng.x,
            y: e.latlng.y
        };
        _markerFunctions.show(menu, map, e.latlng);
    });
}

export function addMarkers(sessionId, markers) {
    //console.log(markers);
    let map = _mapMap.get(sessionId);

    var naverMarkers = new Array(markers.length);
    for (let i = 0; i < markers.length; i++) {
        let naverMarker = new naver.maps.Marker({
            map: map,
            position: new naver.maps.LatLng(markers[i].latitude, markers[i].longitude),
            icon: _markerIcons.location
        });

        naver.maps.Event.addListener(naverMarker, 'click', (e) => {
            //console.log(e);
            selectMarker(sessionId, naverMarker.tag.id);

            let dotNetRef = _dotNetRefMap.get(sessionId);
            if (dotNetRef) {
                dotNetRef.invokeMethodAsync(OnSelectedMarkerChangedId, sessionId, naverMarker.tag.id);
            }
        });

        naverMarker.tag = markers[i];

        naverMarkers[i] = naverMarker;
    }
    _markersMap.set(sessionId, naverMarkers);
}

export function clearMarkers(sessionId) {
    let naverMarkers = _markersMap.get(sessionId);
    if (!naverMarkers)
        return;

    for (let i = 0; i < naverMarkers.length; i++) {
        naverMarkers[i].setMap(null);
    }
}

export function selectMarker(sessionId, markerId) {
    let naverMarkers = _markersMap.get(sessionId);
    if (!naverMarkers)
        return;

    let selectedMarker = _selectedMarkerMap.get(sessionId);
    if (selectedMarker) {
        _markerFunctions.deselect(selectedMarker);
    }

    for (let i = 0; i < naverMarkers.length; i++) {
        if (naverMarkers[i].tag.id == markerId) {
            _markerFunctions.select(naverMarkers[i]);
          
            _selectedMarkerMap.set(sessionId, naverMarkers[i]);
        }
    }
}

export function move(sessionId, latitude, longitude) {
    let map = _mapMap.get(sessionId);
    if (!map)
        return;
    map.setCenter(new naver.maps.LatLng(latitude, longitude));
}

/**
 * 맵을 종료한다
 * */
export function close(sessionId) {
    _dotNetRefMap.delete(sessionId);
    _mapMap.delete(sessionId);
    _myLocationMarkerMap.delete(sessionId);
}

export function getMarkerLocation(sessionId) {
    return _myLocationMarkerMap.get(sessionId).getPosition();
}