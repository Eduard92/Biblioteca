(function () {
    'use strict';

    //angular.module('app.comision', []);


    angular.module('app.empleados')
       
        .directive('ngCamera', [NgCamera])
        
        .controller('InputUpload', ['$scope', '$http', 'Upload', InputUpload])
        .controller('InputCtrlCamera', ['$scope', '$http', '$uibModalInstance', 'camera', InputCtrlCamera])
        .controller('InputCtrl', ['$scope', '$http', '$uibModal', InputCtrl]);

    
    function InputCtrlCamera($scope, $http, $uibModalInstance,camera) {
       // $scope.start = status;
        $scope.fotografia = '';
        $scope.cancel = function () {
            $uibModalInstance.dismiss("cancel");
        }
        $scope.save = function () {

           
        }

        $scope.$watch('camera', function (n, o) {

            if (!n) return false;
            $uibModalInstance.close(n);
        });
        
    }
    function InputUpload($scope, $http, Upload) {
        $scope.empleados = [];
        $scope.dispose = true;
        $scope.upload_file = function (file) {
            $scope.dispose = false;
            if (!file) return false;
            file.upload = Upload.upload({
                url: '/Empleados/FileUpload',
                data: { file: file},
            });
            file.upload.then(function (response) {
                $scope.dispose = true;
                var result = response.data,
                    data = result.data,
                    status = result.status;

                $scope.message = result.message;
                $scope.status = result.status;

                if (result.status) {
                    $scope.empleados = result.data;
                    console.log(result.data);
                }
            });
        }
    }

    function NgCamera() {
        return {



            restrict: 'E',
            require: '^ngModel',
            scope: {
                name: '@',
                start:'='
            },
            controller: function ($scope) {
            },
            template: '<a href="#" id="start-camera" class="visible">Touch here to start the app.</a><video muted id="camera-stream"></video><div id="mask"><img  src="/Content/img/mask_cut.png"/></div><img id="snap" /> <p id="error-message"></p> <div class="controls"><a href="#" id="delete-photo" title="Eliminar foto" class="disabled"><i class="fa fa-trash"></i></a><a href="#" id="take-photo" title="Tomar foto"><i class="fa fa-camera"></i></a><a href="#" id="download-photo" ng-click="save()" download="selfie.png" title="Salvar foto" class="disabled"><i class="fa fa-download"></i></a>  </div><canvas></canvas>',
            link: function propiedades(scope, element, attr, ngModelCtrl) {
                var video = document.querySelector('#camera-stream'),
                   image = document.querySelector('#snap'),
                   start_camera = document.querySelector('#start-camera'),
                   controls = document.querySelector('.controls'),
                   take_photo_btn = document.querySelector('#take-photo'),
                   delete_photo_btn = document.querySelector('#delete-photo'),
                   download_photo_btn = document.querySelector('#download-photo'),
                   error_message = document.querySelector('#error-message'),
                   camera_stream = false;

                var zoom = 1;


                navigator.getMedia = (
                   navigator.getUserMedia ||
                   navigator.webkitGetUserMedia ||
                   navigator.mozGetUserMedia ||
                   navigator.msGetUserMedia
               );

                scope.$on('$destroy', function () {
                    camera_stream.getTracks()[0].stop();
                    console.log('captured $destroy event');
                });
                if (!navigator.getMedia) {
                    displayErrorMessage("Your browser doesn't have support for the navigator.getUserMedia interface.");
                }
                else {
                    navigator.getMedia(
                            {
                                //video: true
                               video: { height:(700*zoom),width:(800*zoom) }
                            },
                            // Success Callback
                            function (stream) {
                                camera_stream = stream;
                                // Create an object URL for the video stream and
                                // set it as src of our HTLM video element.
                                video.src = window.URL.createObjectURL(stream);
                                //video.src = '';

                                // Play the video element to start the stream.
                                //video.pause();
                                video.play();
                                
                                video.onplay = function () {
                                    showVideo();
                                };
                                
                            },
                            // Error Callback
                            function (err) {
                                displayErrorMessage("There was an error with accessing the camera stream: " + err.name, err);
                            }
                        );
                }
                download_photo_btn.addEventListener("click", function (e) {
                    e.preventDefault();
                    var snap = download_photo_btn.href;

                    ngModelCtrl.$setViewValue(snap);
                    
                });
                delete_photo_btn.addEventListener("click", function (e) {

                    e.preventDefault();

                    // Hide image.
                    image.setAttribute('src', "");
                    image.classList.remove("visible");
                   // $('#fotografia_canv').val('');
                    // Disable delete and save buttons
                    delete_photo_btn.classList.add("disabled");
                    download_photo_btn.classList.add("disabled");
                    take_photo_btn.classList.remove('disabled');
                    // Resume playback of stream.
                    video.play();

                });
                take_photo_btn.addEventListener("click", function (e) {

                    e.preventDefault();

                    var snap = takeSnapshot();

                    
                    // Set the href attribute of the download button to the snap url.
                    download_photo_btn.href = snap;

                    video.pause();
                    //video.src = '';
                    //camera_stream.getTracks()[0].stop();
                    

                    // Enable delete and save buttons
                    delete_photo_btn.classList.remove("disabled");
                    download_photo_btn.classList.remove("disabled");

                    take_photo_btn.classList.add('disabled');

                    image.setAttribute('src', snap);
                    image.classList.add("visible");
                });
                scope.$watch('start', function (newValue, oldValue) {
                    /*
                    if (newValue === oldValue) return false;
                   
                   
                    if (newValue)
                        video.play();
                    else {
                        
                        video.pause();
                        camera_stream.getTracks()[0].stop();
                    }*/
                });
               
                function takeSnapshot() {
                    // Here we're using a trick that involves a hidden canvas element.  

                    var hidden_canvas = document.querySelector('canvas'),
                        context = hidden_canvas.getContext('2d');

                    var width = video.videoWidth,
                        height = video.videoHeight;

                    if (width && height) {

                        // Setup a canvas with the same dimensions as the video.
                        hidden_canvas.width = width;
                        hidden_canvas.height = height;

                        // Make a copy of the current frame in the video on the canvas.
                        context.drawImage(video, 0, 0, width, height);

                        // Turn the canvas image into a dataURL that can be used as a src for our photo.
                        return hidden_canvas.toDataURL('image/png');
                    }
                }


                function showVideo() {
                    hideUI();
                    video.classList.add("visible");
                    controls.classList.add("visible");
                }


                function displayErrorMessage(error_msg, error) {
                    error = error || "";
                    if (error) {
                        console.error(error);
                    }

                    error_message.innerText = error_msg;

                    hideUI();
                    error_message.classList.add("visible");
                }


                function hideUI() {
                    // Helper function for clearing the app UI.

                    controls.classList.remove("visible");
                    start_camera.classList.remove("visible");
                    video.classList.remove("visible");
                    snap.classList.remove("visible");
                    error_message.classList.remove("visible");
                }
            }

        }
    }

    
    function InputCtrl($scope, $http, $uibModal) {
        $scope.areas = [];
        $scope.resource = 'image';
        $scope.camera = false;
       
        $scope.remove_camera = function () {
            $scope.camera = false;
        }
        $scope.start_camera = function (start) {
           
            
            var modalInstance = $uibModal.open({
                animation: true,
                templateUrl: 'modalCamera.html',
                controller: 'InputCtrlCamera',
                //size: size,
                resolve: {
                    camera: function () {
                        return $scope.camera;
                    }


                }
            });
            modalInstance.result.then(function (camera) {
                $scope.camera = camera;
               
            }, function () {

                $scope.status = false;

            });
        };
        $scope.$watch('fotografia', function (newValue, oldValue) {
            console.log(newValue);
        });
        $scope.$watch('id_centro', function (newValue, oldValue) {

            if (!newValue) return false;

            $http.get("/Empleados/ListAreas", { params: { id_centro: newValue } }).then(function (response) {
                var result = response.data,
                    areas = result;

                $scope.areas = areas;

            });
        });
    }
})();