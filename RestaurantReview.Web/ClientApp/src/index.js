import "antd/dist/antd.css";
import './index.css';

import React from 'react';
import ReactDOM from 'react-dom';
import { BrowserRouter } from 'react-router-dom';
import { Provider } from "mobx-react";
import App from './App';
import stores from "./stores/stores";
import registerServiceWorker from './registerServiceWorker';
import { ErrorBoundary } from "./components/components";

const baseUrl = document.getElementsByTagName('base')[0].getAttribute('href');
const rootElement = document.getElementById('root');

ReactDOM.render(
    <BrowserRouter basename={baseUrl}>
        <Provider {...stores}>
            <ErrorBoundary>
                <App />
            </ErrorBoundary>
        </Provider>
    </BrowserRouter>,
    rootElement);

registerServiceWorker();
