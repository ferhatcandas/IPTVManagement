// App.js

import React, { Component } from 'react';
import Header from './components/Header';
import SideBar from './components/Sidebar';
import Content from './components/Content';
import Footer from './components/Footer';
// import SideBar from './SideBar';
// import Content from './Content';

export default class App extends Component {

  render() {
    return (
      <div className="wrapper">
        {/* <Header /> */}
        {/* <SideBar /> */}
        <Content />
        {/* <Footer /> */}
      </div>
    );
  }
}
