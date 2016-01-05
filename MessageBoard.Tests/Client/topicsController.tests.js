/// <reference path="../scripts/_references.js" />
/// <reference path="../../messageboard/js/home.js" />

describe("topicsController ->", function () {


    beforeEach(function () {

        module("messageBoard");

    });

    var $httpBackend;

    beforeEach(inject(function($injector) {

        $httpBackend = $injector.get("$httpBackend");

        $httpBackend.when("GET", "/api/v1/topics?includeReplies=true")
            .respond([
                {
                    title: "first title",
                    body: "some body",
                    id: 1,
                    created: "20151201"
                },
                {
                    title: "second title",
                    body: "some body",
                    id: 1,
                    created: "20151201"
                },
                {
                    title: "third title",
                    body: "some body",
                    id: 1,
                    created: "20151201"
                }
            ]);
    }));

    afterEach(function () {

        // Pour que les tests s'exécutent de façon indépendante
        $httpBackend.verifyNoOutstandingExpectation();
        $httpBackend.verifyNoOutstandingRequest();

    });

    describe("dataService ->", function() {

        it("can load topics", inject(function (dataService) {

            expect(dataService.topics).toEqual([]);

            $httpBackend.expectGET("/api/v1/topics?includeReplies=true");
            dataService.getTopics();
            $httpBackend.flush();

            expect(dataService.topics.length).toBeGreaterThan(0);
            expect(dataService.topics.length).toEqual(3);

        }));

    });

    describe("topicsController ->", function() {

        it("load data", inject(function ($controller, dataService) {

            $httpBackend.expectGET("/api/v1/topics?includeReplies=true");

            var ctrl = $controller("topicsController", {
                dataService: dataService
            });

            $httpBackend.flush();

            expect(ctrl).not.toBeNull();
            expect(ctrl.data).toBeDefined();

        }));

    });

});