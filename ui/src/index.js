import React from 'react';
import ReactDOM from 'react-dom';
import App from './components/App';
import { ThemeProvider } from "@material-ui/styles";
import { CssBaseline } from "@material-ui/core";
import theme from "./themes/index";


ReactDOM.render(
  <ThemeProvider theme={theme} >
    <CssBaseline />
    <App />
  </ThemeProvider>,
  document.getElementById('root')
);
