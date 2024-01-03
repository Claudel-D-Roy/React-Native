import React from 'react';
import { createRoot } from 'react-dom/client';
import App from './components/app/App';
import reportWebVitals from './components/utils/reportWebVitals';
import * as serviceWorkerRegistration from './components/utils/serviceWorkerRegistration';
import 'bootstrap/dist/css/bootstrap.css';
import './styles/index.css';

const rootElement = document.getElementById('root');
const root = createRoot(rootElement);
// Remove StrictMode to avoid double rendering, keep it while in development!!!
root.render(
  <React.StrictMode>
    <App />
  </React.StrictMode>
);

// If you want your app to work offline and load faster, you can change
// unregister() to register() below. Note this comes with some pitfalls.
// Learn more about service workers: https://cra.link/PWA
serviceWorkerRegistration.unregister();

// If you want to start measuring performance in your app, pass a function
// to log results (for example: reportWebVitals(console.log))
// or send to an analytics endpoint. Learn more: https://bit.ly/CRA-vitals
reportWebVitals();
