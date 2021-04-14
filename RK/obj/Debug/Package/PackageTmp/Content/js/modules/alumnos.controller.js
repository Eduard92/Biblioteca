(function () {
    'use strict';

    


    angular.module('app.alumnos')
    .directive('openModal', ['$uibModal', '$http', OpenModal])
    .controller('IndexCtrl', ['$scope', '$http', '$uibModal', IndexCtrl])
    .controller('InputCtrl', ['$scope', '$http', '$uibModalInstance', 'Upload', InputCtrl]);
    
    function InputCtrl($scope, $http, $uibModalInstance, Upload) {
        $scope.dispose = true;
        $scope.cancel = function () {
            $uibModalInstance.dismiss("cancel");
        }

        $scope.upload_file = function (file) {

            if (!file) return false;
            $scope.dispose = false;
            file.upload = Upload.upload({
                url: '/Alumnos/FileUpload',
                data: { file: file },
            });
            file.upload.then(function (response) {
                $scope.dispose = true;
                var result = response.data,
                    data = result.data,
                    status = result.status;


                console.log(result);

                $scope.message = result.message;
                $scope.status = result.status;

                if ($scope.status) {
                    location.href = SITE_URL+'alumnos';
                }

                
            }, function (response) {
                $scope.dispose = true;
                if (response.status > 400) {
                    $scope.status = false;
                    $scope.message = 'Error: ' + response.status;
                }

            }, function (evt) {

                file.progress = Math.min(100, parseInt(100.0 * evt.loaded / evt.total));
            });
        }
    }
    function IndexCtrl($scope,$http,$uibModal) {
        $scope.upload = function () {
            return $uibModal.open({

                templateUrl: 'modalUpload.html',
                controller:   'InputCtrl',

                resolve: {
                    
                }
            });
        }
    }
    function OpenModal($uibModal,$http) {

        return {

            restrict: 'A',
            scope: {
               // modalController: '@',
                title:'@'
            },
            link: function propiedades(scope, element, attr) {
                element.bind('click', function (e) {
                    var url = element.attr('href');
                    e.preventDefault();

                    return $uibModal.open({
                        
                        templateUrl: 'myModal.html',
                        controller: ['$scope', function ($scope) {
                            $scope.title = attr.title;

                            $http.get(url).then(function (response) {

                                $scope.modal_body = response.data;
                            });
                        }],                       
                        resolve: {
                            /*title: function() {
                                return attr.title;
                            },
                            url: function () {
                               
                                return $http.get(url);
                            }*/
                        }
                    });

                    //container.html($compile(loadedHtml)(scope))
                   // $rootScope.$broadcast('preloader:active');
                    //$myModal.open(element.attr('href'), attr.modalTitle, attr.modalController, attr.template);



                });

            }


        }
    }
    
    
})();