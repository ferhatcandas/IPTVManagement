import React, { Component } from 'react';
export default class Footer extends Component {
    render() {
        return (
            <footer className="main-footer">
                <div className="float-right d-none d-sm-block">
                    <b>Version</b> 3.1.0-rc
        </div>
                <strong>Copyright &copy; 2014-2020 <a href="/#">AdminLTE.io</a>.</strong> All rights reserved.
            </footer>
        )
    }
}