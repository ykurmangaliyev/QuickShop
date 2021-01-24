"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.getUserClaims = exports.getCurrentToken = void 0;
const jwt_decode_1 = require("jwt-decode");
const universal_cookie_1 = require("universal-cookie");
const cookieName = "JwtBearerAuthentication";
function getCurrentToken() {
    return new universal_cookie_1.default().get(cookieName);
}
exports.getCurrentToken = getCurrentToken;
function getUserClaims() {
    const cookie = getCurrentToken();
    const decoded = jwt_decode_1.default(cookie);
    return {
        issuedAt: decoded.iat,
        expireAt: decoded.ist,
        id: decoded.id,
        username: decoded.nameid,
    };
}
exports.getUserClaims = getUserClaims;
//# sourceMappingURL=authentication.js.map