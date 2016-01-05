/// <reference path="../scripts/_references.js" />
/// <reference path="../../messageboard/js/myapp.js" />

describe("myapp ->", function () {

    it("isDebug", function() {
        expect(app.isDebug).toEqual(true);
    });

    it("log", function () {
        expect(app.log).toBeDefined();
        app.log("testing");
    });

});