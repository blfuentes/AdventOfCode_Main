"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.Day03Part02 = void 0;
const Claim_1 = require("../Claim");
class Day03Part02 {
    execute() {
        let fs = require("fs");
        let path = require('path');
        let entryElements = [];
        let filepath = path.join(__dirname, "../day03_input.txt");
        // let filepath = path.join(__dirname, "../test01_input.txt");
        let text = fs.readFileSync(filepath, "utf-8");
        entryElements = text.split("\r\n");
        let claimsList = [];
        for (let entry of entryElements) {
            claimsList.push(new Claim_1.Claim(entry));
        }
        let baseSquare;
        baseSquare = Array(1000).fill(null).map(item => (new Array(1000).fill(".")));
        // baseSquare = new Array(1000);
        // for (var idx = 0; idx < baseSquare.length; idx++) {
        //     baseSquare[idx] = new Array(1000);
        //     for (var jdx = 0; jdx < baseSquare.length; jdx++) {
        //         baseSquare[idx][jdx] = ".";
        //     }
        // }
        let squareInches = 0;
        let uniqueClaimId = 0;
        for (let claim of claimsList) {
            for (var cidx = claim.initTop; cidx < (claim.initTop + claim.height); cidx++) {
                for (var cjdx = claim.initLeft; cjdx < (claim.initLeft + claim.width); cjdx++) {
                    if (baseSquare[cidx][cjdx] === ".") {
                        baseSquare[cidx][cjdx] = claim.id.toString();
                    }
                    else {
                        if (baseSquare[cidx][cjdx] !== "X") {
                            squareInches++;
                        }
                        baseSquare[cidx][cjdx] = "X";
                    }
                }
            }
        }
        let isUniqueClaim = true;
        for (let claim of claimsList) {
            isUniqueClaim = true;
            for (var cidx = claim.initTop; cidx < (claim.initTop + claim.height); cidx++) {
                for (var cjdx = claim.initLeft; cjdx < (claim.initLeft + claim.width); cjdx++) {
                    if (baseSquare[cidx][cjdx] === "X") {
                        isUniqueClaim = false;
                        break;
                    }
                }
                if (!isUniqueClaim) {
                    break;
                }
            }
            if (isUniqueClaim) {
                uniqueClaimId = claim.id;
            }
        }
        //console.log(`Overlaped: ${squareInches}. UniqueClaim: ${uniqueClaimId}.`);
        return uniqueClaimId;
    }
}
exports.Day03Part02 = Day03Part02;
