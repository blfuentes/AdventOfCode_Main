"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.Node = void 0;
class Node {
    constructor(value, parent) {
        this.element = value;
        this.parentNodes = [];
        if (parent != undefined) {
            this.parentNodes.push();
        }
        this.childNodes = [];
        this.duration = this.element.charCodeAt(0) - 64;
    }
}
exports.Node = Node;
//# sourceMappingURL=Node.js.map