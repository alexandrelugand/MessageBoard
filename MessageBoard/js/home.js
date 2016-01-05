(function () {
    'use strict';

    var messageBoardModule = angular.module("messageBoard", ['ngRoute']);

    messageBoardModule.config(["$routeProvider", function ($routeProvider) {
        $routeProvider
            .when("/", {
                controller: "topicsController",
                controllerAs: "home",
                templateUrl: "/templates/topicsView.html"
            })
            .when("/newmessage", {
                controller: "newTopicController",
                controllerAs: "newTopic",
                templateUrl: "/templates/newTopicView.html"
            })
            .when("/message/:id", {
                controller: "singleTopicController",
                controllerAs: "singleTopic",
                templateUrl: "/templates/singleTopicView.html"
            })
            .otherwise({ redirectTo: "/" });
    }]);

    messageBoardModule.factory("dataService", ["$http", "$q", function ($http, $q) {

        var _topics = [];
        var _isInit = false;

        var _isReady = function() {
            return _isInit;
        };

        var _getTopics = function () {

            var deferred = $q.defer();

            $http.get("/api/v1/topics?includeReplies=true")
                .then(function(result) {
                        // Success
                        angular.copy(result.data, _topics);
                        _isInit = true;
                        deferred.resolve();
                    },
                    function() {
                        // Error
                        deferred.reject();
                    });

            return deferred.promise;

        };

        var _addTopic = function(newTopic) {

            var deferred = $q.defer();

            $http.post("/api/v1/topics", newTopic)
              .then(function (result) {
                      // Success
                      var topic = result.data;
                      _topics.splice(0, 0, topic);
                      deferred.resolve(topic);
                  },
                  function () {
                      // Error
                      deferred.reject();
                  });

            return deferred.promise;
        };

        function _findTopic(id) {
            var found = null;

            $.each(_topics, function(i, item) {
               if (item.id == id) {
                   found = item;
                   return false;
               }
            });

            return found;
        }

        var _getTopicById = function(id) {

            var deferred = $q.defer();

            if (_isReady()) {
                var topic = _findTopic(id);
                if (topic) {
                    deferred.resolve(topic);
                } else {
                    deferred.reject();
                }
            } else {
                _getTopics()
                    .then(function() {
                        //Success
                        var topic = _findTopic(id);
                        if (topic) {
                            deferred.resolve(topic);
                        } else {
                            deferred.reject();
                        }
                    },
                    function () {
                        //Error
                        deferred.reject();
                    });
            }

            return deferred.promise;
        };

        var _saveReply = function(topic, reply) {

            var deferred = $q.defer();

            $http.post("/api/v1/topics/" + topic.id + "/replies", reply)
                .then(function (result) {
                        // Success
                        if (topic.replies == null)
                            topic.replies = [];
                        topic.replies.push(result.data);
                        deferred.resolve(result.data);
                    },
                    function () {
                        // Error
                        deferred.reject();
                    });

            return deferred.promise;
        };

        return {
            topics: _topics,
            getTopics: _getTopics,
            addTopic: _addTopic,
            isReady: _isReady,
            getTopicById: _getTopicById,
            saveReply: _saveReply
        };
    }]);
                   

    messageBoardModule.controller("topicsController", ["dataService", function (dataService) {

        var vm = this;  
        vm.data = dataService;
        vm.isBusy = false;

        if (!dataService.isReady()) {
            vm.isBusy = true;

            dataService.getTopics()
                .then(function(result) {
                        // Success
                    },
                    function() {
                        // Error
                        alert("Could not load topics");
                    })
                .then(function() {
                    vm.isBusy = false;
                });
        }
    }]);

    messageBoardModule.controller("newTopicController", ["$window", "dataService", function ($window, dataService) {

        var vm = this;
        vm.topic = {};

        vm.save = function() {

            dataService.addTopic(vm.topic)
                .then(function(result) {
                        // Success
                        $window.location = "#/";
                    },
                    function() {
                        // Error
                        alert("Cannot save the new topic");
                    });

        };
    }]);

    messageBoardModule.controller("singleTopicController", ["$window", "dataService", "$routeParams", function ($window, dataService, $routeParams) {

        var vm = this;
        vm.topic = null;
        vm.reply = {};

        dataService.getTopicById($routeParams.id)
            .then(function(topic) {
                //Success
                    vm.topic = topic;
                },
                function() {
                    //Error
                    $window.location = "#/";
                });

        vm.addReply = function () {

            dataService.saveReply(vm.topic, vm.reply)
                .then(function (result) {
                    // Success
                    vm.reply.body = "";
                },
                function () {
                    // Error
                    alert("Cannot save the new reply");
                });

        };
    }]);

})();