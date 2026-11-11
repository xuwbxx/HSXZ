var camera, scene, renderer;

var isUserInteracting = false,
    onMouseDownMouseX = 0, onMouseDownMouseY = 0,
    lon = 0, onMouseDownLon = 0,
    lat = 0, onMouseDownLat = 0,
    phi = 0, theta = 0;

var initViewObj = {};

var raycaster = new THREE.Raycaster();
var pointObjs = new Array();
var mouse = new THREE.Vector2();
var container;
var selectedObject = null;
var selectedMaterial = null;

/**
 * Draws a rounded rectangle using the current state of the canvas.
 * @param {Number} x The top left x coordinate
 * @param {Number} y The top left y coordinate
 * @param {Number} w The width of the rectangle
 * @param {Number} h The height of the rectangle
 * @param {Number} r The corner radius;
 */
CanvasRenderingContext2D.prototype.roundRect = function (x, y, w, h, r) {
    var min_size = Math.min(w, h);

    if (r > min_size / 2)
    {
        r = min_size / 2;
    }

    // 开始绘制
    this.beginPath();
    this.moveTo(x + r, y);
    this.arcTo(x + w, y, x + w, y + h, r);
    this.arcTo(x + w, y + h, x, y + h, r);
    this.arcTo(x, y + h, x, y, r);
    this.arcTo(x, y, x + w, y, r);
    this.closePath();

    return this;
}

function init() {

    //var container, mesh;
    var mesh;

    container = document.getElementById('container');

    camera = new THREE.PerspectiveCamera(75, window.innerWidth / window.innerHeight, 1, 1100);
    camera.target = new THREE.Vector3(0, 0, 0);
    mouse = new THREE.Vector2();
    raycaster = new THREE.Raycaster();
    pointObjs = new Array();
    scene = new THREE.Scene();

    var ImgUrl = $('#hid_BackGroundPic').val();

    var geometry = new THREE.SphereBufferGeometry(500, 60, 40);
    // invert the geometry on the x-axis so that all of the faces point inward
    geometry.scale(-1, 1, 1);

    var material = new THREE.MeshBasicMaterial({
        map: new THREE.TextureLoader().load(ImgUrl)
    });

    mesh = new THREE.Mesh(geometry, material);

    scene.add(mesh);

    renderer = new THREE.WebGLRenderer({ canvas: container });

    renderer.setPixelRatio(window.devicePixelRatio);
    renderer.setSize(window.innerWidth, window.innerHeight);

    var light = new THREE.DirectionalLight(0xffffff); //添加了一个白色的平行光
    light.position.set(10, 50, 50); //设置光的方向
    scene.add(light); //添加到场景

    //添加一个全局环境光
    scene.add(new THREE.AmbientLight(0x222222));

    document.addEventListener('mousedown', onDocumentMouseDown, false);
    document.addEventListener('mousemove', onDocumentMouseMove, false);
    document.addEventListener('mouseup', onDocumentMouseUp, false);
   // document.addEventListener('click', onMouseClick, false);
    
    document.addEventListener('wheel', onDocumentMouseWheel, false);

    renderer.render(scene, camera);

    //声明raycaster和mouse变量
    //var raycaster = new THREE.Raycaster();
    //var mouse = new THREE.Vector2();

    var isClick = $('#hid_isClick').val();


    if (isClick === 'true') {
        //添加点击事件
        document.addEventListener('click', function (event) {
            //通过鼠标点击的位置计算出raycaster所需要的点的位置，以屏幕中心为原点，值的范围为-1到1.

            // 屏幕坐标转标准设备坐标

            let x = ((event.clientX - container.getBoundingClientRect().left) / container.offsetWidth) * 2 - 1;// 标准设备横坐标

            // 这里的container是个dom元素,getBoundingClientRectangle会返回当前元素的视口大小.

            let y = -((event.clientY - container.getBoundingClientRect().top) / container.offsetHeight) * 2 + 1;// 标准设备纵坐标

            let standardVector = new THREE.Vector3(x, y, 1);// 标准设备坐标

            // 标准设备坐标转世界坐标

            let worldVector = standardVector.unproject(camera);

            // 射线投射方向单位向量(worldVector坐标减相机位置坐标)

            let ray = worldVector.sub(camera.position).normalize();

            // 创建射线投射器对象

            let rayCaster = new THREE.Raycaster(camera.position, ray);

            // 返回射线选中的对象 第二个参数如果不填 默认是false

            let intersects = rayCaster.intersectObjects(scene.children, true);

            //查找所有的相交的模型，如果只需要将第一个触发事件，那就数组的第一个模型改变颜色即可
            for (var i = 0; i < intersects.length; i++) {
                pointArr.forEach(function (dom, index) {
                    if (dom.UID === intersects[i].object.uuid) {
                        if (dom.Method) {
                            if (dom.params) {
                                eval(pointInfo.Method + "(" + dom.params + ")");
                            }
                            else {
                                eval(pointInfo.Method + "()");
                            }
                        }
                    }
                });
            }
        }, false);
    }


    document.addEventListener('dragover', function (event) {

        event.preventDefault();
        event.dataTransfer.dropEffect = 'copy';

    }, false);

    document.addEventListener('dragenter', function (event) {

        document.body.style.opacity = 0.5;

    }, false);

    document.addEventListener('dragleave', function (event) {

        document.body.style.opacity = 1;

    }, false);

    document.addEventListener('drop', function (event) {

        event.preventDefault();

        var reader = new FileReader();
        reader.addEventListener('load', function (event) {

            material.map.image.src = event.target.result;
            material.map.needsUpdate = true;

        }, false);
        reader.readAsDataURL(event.dataTransfer.files[0]);

        document.body.style.opacity = 1;

    }, false);

    window.addEventListener('resize', onWindowResize, false);
}

function onWindowResize() {

    camera.aspect = window.innerWidth / window.innerHeight;
    camera.updateProjectionMatrix();

    renderer.setSize(window.innerWidth, window.innerHeight);

}

function onDocumentMouseDown(event) {

    //event.preventDefault();

    isUserInteracting = true;

    onMouseDownMouseX = event.clientX;
    onMouseDownMouseY = event.clientY;

    onMouseDownLon = lon;
    onMouseDownLat = lat;

    initView = false;
}

//function onMouseClick(event) {

//    //通过鼠标点击的位置计算出raycaster所需要的点的位置，以屏幕中心为原点，值的范围为-1到1.

//    mouse.x = (event.clientX / window.innerWidth) * 2 - 1;
//    mouse.y = -(event.clientY / window.innerHeight) * 2 + 1;

//    // 通过鼠标点的位置和当前相机的矩阵计算出raycaster
//    raycaster.setFromCamera(mouse, camera);

//    // 获取raycaster直线和所有模型相交的数组集合
//    var intersects = raycaster.intersectObjects(scene.children);

//    console.log(intersects);

//    ////将所有的相交的模型的颜色设置为红色，如果只需要将第一个触发事件，那就数组的第一个模型改变颜色即可
//    for (var i = 0; i < intersects.length; i++) {
//        // intersects[i].object.material.color.set(0xff0000);

//        //for (var i = 0; i < intersects.length; i++) {
//            pointArr.forEach(function (dom, index) {
//                if (dom.UID === intersects[i].object.uuid) {
//                    RedirectPage(dom);
//                }
//            });
//        //}

//    }

//}
//window.addEventListener('click', onMouseClick, false);

function onDocumentMouseMove(event) {

    if (isUserInteracting === true) {

        lon = (onMouseDownMouseX - event.clientX) * 0.1 + onMouseDownLon;
        lat = (event.clientY - onMouseDownMouseY) * 0.1 + onMouseDownLat;

    }
   // mouse.x = (event.clientX / window.innerWidth) * 2 - 1;
   // mouse.y = -(event.clientY / window.innerHeight) * 2 + 1;

    mouse.x = ((event.clientX - container.getBoundingClientRect().left) / container.offsetWidth) * 2 - 1;// 标准设备横坐标
    // 这里的container是个dom元素,getBoundingClientRectangle会返回当前元素的视口大小.
    mouse.y = -((event.clientY - container.getBoundingClientRect().top) / container.offsetHeight) * 2 + 1;// 标准设备纵坐标
}

function onDocumentMouseUp(event) {

    isUserInteracting = false;

}

function onDocumentMouseWheel(event) {

    var fov = camera.fov + event.deltaY * 0.05;

    camera.fov = THREE.Math.clamp(fov, 10, 75);

    camera.updateProjectionMatrix();

}

function animate() {
    requestAnimationFrame(animate);
    update();
}

var initView = true;

function update() {

    //if ( isUserInteracting === false ) {

    //	lon += 0.1;

    //}

    //lat = Math.max(-85, Math.min(85, lat));
    //phi = THREE.Math.degToRad(90 - lat);
    //theta = THREE.Math.degToRad(lon);

    //camera.target.x = 500 * Math.sin(phi) * Math.cos(theta);
    //camera.target.y = 500 * Math.cos(phi);
    //camera.target.z = 500 * Math.sin(phi) * Math.sin(theta);

    if (initView) {
        camera.target.x = initViewObj.x;
        camera.target.y = initViewObj.y;
        camera.target.z = initViewObj.z;
    }
    else {
        lat = Math.max(-85, Math.min(85, lat));
        phi = THREE.Math.degToRad(90 - lat);
        theta = THREE.Math.degToRad(lon);

        camera.target.x = 500 * Math.sin(phi) * Math.cos(theta);
        camera.target.y = 500 * Math.cos(phi);
        camera.target.z = 500 * Math.sin(phi) * Math.sin(theta);
    }

    camera.lookAt(camera.target);

    /*
    // distortion
    camera.position.copy( camera.target ).negate();
    */

    raycaster.setFromCamera(mouse, camera);
    var intersects = raycaster.intersectObjects(pointObjs, true);
    if (intersects.length > 0) {
        setStatus();
    }
    else {
        restoreObjectStatus();
    }

    renderer.render(scene, camera);
}


function setStatus() {
    container.style.cursor = "pointer";
}


function restoreObjectStatus() {
    container.style.cursor = "default";
}


var radius = 0;

function InitView(x, y, z) {
    //相机坐标
    var pos1 = new THREE.Vector3(0, 0, 0);
    //初始点坐标
    var pos2 = new THREE.Vector3(x, y, z);
    //x与z轴平面上初始点的映射坐标
    var pos3 = new THREE.Vector3(x, 0, z);

    initViewObj.x = x;
    initViewObj.y = y;
    initViewObj.z = z;

    //相机与初始点距离
    radius = pos1.distanceTo(pos2);
    //相机与初始点映射坐标的距离
    radius3 = pos1.distanceTo(pos3);
    //维度弧度
    radLat = Math.asin(y / radius);
    //经度弧度
    radLon = Math.asin(z / radius3);
    //维度角度
    lat = THREE.Math.radToDeg(radLat);
    //经度角度
    lon = THREE.Math.radToDeg(radLon);
}

//创建canvas对象
function drawCanvas(options) {

    let canvas = document.createElement("canvas");
    canvas.width = options.width;
    canvas.height = options.height;

    let ctx = canvas.getContext("2d");

    ctx.fillStyle = "rgba(0, 0, 0, 0.4)";
    ctx.roundRect(0, 0, canvas.width, canvas.height, 15);
    ctx.fill();

    ctx.font = "32px 微软雅黑";
    ctx.textAlign = "left";    
    ctx.fillStyle = "#ffffff";
    //if (options.text.search("靠船力") != -1)
    //{
    //    options.width = 350;
    //}

    ctx.fillText(options.text, 10, 45, options.width);

    return canvas;
}

var pointArr = new Array();
//MonitorArea模型
function CreatePoint(dom) {
   // pointArr = new Array();
    //var x = dom.X;
    //var z = dom.Y;
    //var y = -14;

    var x = dom.NewMapX - 0;
    var y = dom.NewMapY - 0;
    var z = dom.NewMapZ - 0;

    //if (dom.Z) {
    //    y = dom.Z;
    //}

    var iconName = 'point1';

    if (dom.IconClass) {
        iconName = dom.IconClass;
    } 

    var mater = new THREE.SpriteMaterial({
        map: new THREE.TextureLoader().load("../../Content/New/img/" + iconName + ".png")
    });

    mesh = new THREE.Sprite(mater);
    
    //mesh.position.set(x, -18, z); //设置位置
    mesh.position.set(x, y, z); //设置位置
    mesh.scale.set(6, 6, 6); //设置scale进行大小设置
    pointObjs.push(mesh);
    scene.add(mesh);

    
    pointInfo = {};
    pointInfo.UID = mesh.uuid;
    pointInfo.MonitorAreaName = dom.MonitorAreaName;
    pointInfo.MonitorAreaID = dom.MonitorAreaID;
    
    if (dom.method) {
        pointInfo.Method = dom.method;
    }

    //动态生成参数,params参数提供给click事件使用。
    if (dom.params) {
        //pointInfo.params = dom.params;
        pointInfo.params = "";
        var params = dom.params.split(",");
        params.forEach(function (elem, index) {
            if (index === 0) {
                pointInfo.params += "dom." + elem;
            }
            else {
                pointInfo.params += ",dom." + elem;
            }

            pointInfo[elem] = dom[elem];
        });
    }

    pointArr.push(pointInfo);

    //添加一个精灵 使用了将canvas生成img的src导入的方式
    var sriteMap = [];
    if(dom.MonitorAreaName.search("靠船力")!=-1)
    {
        spriteMap = new THREE.TextureLoader().load(drawCanvas({ text: dom.MonitorAreaName, width: 300, height: 64 }).toDataURL());
    }
    else
    {
        spriteMap = new THREE.TextureLoader().load(drawCanvas({ text: dom.MonitorAreaName, width: 200, height: 64 }).toDataURL());
    }
    
    
    var spriteMaterial = new THREE.SpriteMaterial({ map: spriteMap, color: 0xffffff });
    var sprite = new THREE.Sprite(spriteMaterial);

    //sprite.position.set(58, -14, 21); //设置位置134
    //sprite.position.set(x, -14, z); //设置位置134
    var tempy = y + 3.5;
    sprite.position.set(x, tempy, z); //设置位置134
    sprite.scale.set(10, 4, 4); //设置scale进行大小设置

    scene.add(sprite);
}

function SetClickEnable(isClick) {
    isEnClick = isClick;
    if (isEnClick) {
        document.addEventListener('click', onDocumentMouseClick, false);
    }
    else {
        document.removeEventListener('click', onDocumentMouseClick, false);
    }
}

function onDocumentMouseClick(event) {
    //通过鼠标点击的位置计算出raycaster所需要的点的位置，以屏幕中心为原点，值的范围为-1到1.

    // 屏幕坐标转标准设备坐标

    //let x = ((event.clientX - container.getBoundingClientRect().left) / container.offsetWidth) * 2 - 1;// 标准设备横坐标

    //// 这里的container是个dom元素,getBoundingClientRectangle会返回当前元素的视口大小.

    //let y = -((event.clientY - container.getBoundingClientRect().top) / container.offsetHeight) * 2 + 1;// 标准设备纵坐标
    let x = (event.clientX / window.innerWidth) * 2 - 1;
    let y = -(event.clientY / window.innerHeight) * 2 + 1;
    let standardVector = new THREE.Vector3(x, y, 1);// 标准设备坐标

    // 标准设备坐标转世界坐标

    let worldVector = standardVector.unproject(camera);

    // 射线投射方向单位向量(worldVector坐标减相机位置坐标)

    let ray = worldVector.sub(camera.position).normalize();

    // 创建射线投射器对象

    let rayCaster = new THREE.Raycaster(camera.position, ray);

    // 返回射线选中的对象 第二个参数如果不填 默认是false

    //let intersects = rayCaster.intersectObjects(scene.children, true);
    let intersects = rayCaster.intersectObjects(pointObjs, true);

    //查找所有的相交的模型，如果只需要将第一个触发事件，那就数组的第一个模型改变颜色即可
    for (var i = 0; i < intersects.length; i++) {
        pointArr.forEach(function (dom, index) {
            if (dom.UID === intersects[i].object.uuid) {
                RedirectPage(dom);
            }
        });
    }
}

function RedirectPage(dom) {
    //window.open("/Scan/HomePage/Position?ID=" + dom.ID + "&LayerId=" + dom.LayerId);
    //window.location.href = "/Monitor/Main/Position?ID=" + dom.ID + "&LayerId=" + dom.LayerId;
    window.open("/Monitor/Main/MonitorDetail?MonitorAreaID=" + dom.MonitorAreaID);
}