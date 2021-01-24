"use strict";
var __awaiter = (this && this.__awaiter) || function (thisArg, _arguments, P, generator) {
    function adopt(value) { return value instanceof P ? value : new P(function (resolve) { resolve(value); }); }
    return new (P || (P = Promise))(function (resolve, reject) {
        function fulfilled(value) { try { step(generator.next(value)); } catch (e) { reject(e); } }
        function rejected(value) { try { step(generator["throw"](value)); } catch (e) { reject(e); } }
        function step(result) { result.done ? resolve(result.value) : adopt(result.value).then(fulfilled, rejected); }
        step((generator = generator.apply(thisArg, _arguments || [])).next());
    });
};
Object.defineProperty(exports, "__esModule", { value: true });
exports.ping = exports.auth = exports.UnauthorizedError = void 0;
function makeRequest(url, method, body = null) {
    return __awaiter(this, void 0, void 0, function* () {
        const result = yield fetch(url, {
            method: method,
            body: body ? JSON.stringify(body) : null,
            headers: {
                "Content-Type": "application/json"
            }
        });
        console.log(result);
        let responseBody = {};
        try {
            responseBody = yield result.json();
        }
        catch (e) {
            console.error("Cannot deserialize server response, probably something is wrong...", e);
        }
        if (result.status === 401)
            throw new UnauthorizedError("401 Unauthorized");
        return Object.assign({ status: result.status, ok: result.ok }, responseBody);
    });
}
// Errors
class UnauthorizedError extends Error {
    constructor(message) {
        super(message);
    }
}
exports.UnauthorizedError = UnauthorizedError;
exports.auth = {
    signIn(username, password) {
        return makeRequest("/api/auth", "POST", { username, password });
    },
    signOut() {
        return makeRequest("/api/auth", "DELETE");
    }
};
;
exports.ping = {
    ping() {
        return makeRequest("/api/ping", "POST");
    }
};
//# sourceMappingURL=client.js.map