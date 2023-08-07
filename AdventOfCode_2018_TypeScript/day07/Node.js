"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.Node = void 0;
var Node = /** @class */ (function () {
    function Node(value, parent) {
        this.element = value;
        this.parentNodes = [];
        if (parent != undefined) {
            this.parentNodes.push();
        }
        this.childNodes = [];
        this.duration = this.element.charCodeAt(0) - 64;
    }
    return Node;
}());
exports.Node = Node;
