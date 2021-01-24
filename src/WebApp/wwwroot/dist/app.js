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
const React = require("react");
const ReactDOM = require("react-dom");
const authentication_1 = require("./helper/authentication");
const client_1 = require("./api/client");
;
class App extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            auth: {
                token: authentication_1.getCurrentToken(),
            }
        };
        this.handleSignIn = this.handleSignIn.bind(this);
        this.handleSignOut = this.handleSignOut.bind(this);
        this.handlePing = this.handlePing.bind(this);
    }
    componentDidMount() {
        window.onerror = (error) => {
            console.log("hm...");
            if (error instanceof client_1.UnauthorizedError) {
                console.log("Caught global UnauthorizedError!");
                this.setState({
                    auth: {
                        token: null,
                    }
                });
            }
        };
    }
    handlePing() {
        return __awaiter(this, void 0, void 0, function* () {
            let result = yield client_1.ping.ping();
            if (result.ok) {
                console.log(result);
            }
        });
    }
    handleSignIn() {
        return __awaiter(this, void 0, void 0, function* () {
            let result = yield client_1.auth.signIn("first", "password");
            if (result.ok && result.resultCode === "Success") {
                this.setState({
                    auth: {
                        token: JSON.stringify(authentication_1.getUserClaims()),
                    },
                });
            }
        });
    }
    handleSignOut() {
        return __awaiter(this, void 0, void 0, function* () {
            let result = yield client_1.auth.signOut();
            if (result.ok) {
                this.setState({
                    auth: {
                        token: null,
                    },
                });
            }
        });
    }
    render() {
        if (this.state.auth.token == null) {
            return (React.createElement("div", null,
                React.createElement("button", { onClick: this.handleSignIn }, "SignIn"),
                React.createElement("button", { onClick: this.handlePing }, "Ping")));
        }
        ;
        return (React.createElement(React.Fragment, null,
            React.createElement("div", null,
                "Hola! Your token is ",
                this.state.auth.token),
            React.createElement("button", { onClick: this.handleSignOut }, "SignOut"),
            React.createElement("button", { onClick: this.handlePing }, "Ping")));
    }
}
ReactDOM.render(React.createElement(React.StrictMode, null,
    React.createElement(App, null)), document.getElementById("app"));
//# sourceMappingURL=app.js.map