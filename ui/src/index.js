import React from 'react';
import ReactDOM from 'react-dom';
import App from './App.js';
import { createStore } from "redux";
import { Provider } from "react-redux";
import mReducer from "./reducers/mReducer";

const store = createStore(mReducer)
ReactDOM.render(
    <Provider store={store}>
        <App />
    </Provider>,
    document.getElementById('app'));
