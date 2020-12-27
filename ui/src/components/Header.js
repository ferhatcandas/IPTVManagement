import { Component } from 'react';
import { connect } from 'react-redux'

class Header extends Component {
    render() {
        return (
            <nav className="main-header navbar navbar-expand navbar-white navbar-light" >
                <ul className="navbar-nav">
                    {/* <li className="nav-item">
                        <a href="#" className="nav-link" data-widget="pushmenu" role="button"><i className="fas fa-bars"></i></a>
                    </li> */}
                    <li className="nav-item d-none d-sm-inline-block">
                        <a href="#" onClick={() => this.props.changeTab("tvChannels")} className="nav-link">TV Channels</a>
                    </li>
                    <li className="nav-item d-none d-sm-inline-block">
                        <a href="#" onClick={() => this.props.changeTab("integrations")} className="nav-link">Integrations</a>
                    </li>
                </ul>

                <ul className="navbar-nav ml-auto">
                    {/* <li className="nav-item dropdown">
                        <a href="#" className="nav-link" data-toggle="dropdown">
                            <i className="far fa-comments"></i>
                            <span className="badge badge-danger navbar-badge">3</span>
                        </a>
                        <div className="dropdown-menu dropdown-menu-lg dropdown-menu-right">
                            <a href="#" className="dropdown-item">
                                <div className="media">
                                    <img src="assets/img/user1-128x128.jpg" alt="User Avatar" className="img-size-50 mr-3 img-circle" />
                                    <div className="media-body">
                                        <h3 className="dropdown-item-title">
                                            Brad Diesel
                  <span className="float-right text-sm text-danger"><i className="fas fa-star"></i></span>
                                        </h3>
                                        <p className="text-sm">Call me whenever you can...</p>
                                        <p className="text-sm text-muted"><i className="far fa-clock mr-1"></i> 4 Hours Ago</p>
                                    </div>
                                </div>
                            </a>
                            <div className="dropdown-divider"></div>
                            <a href="#" className="dropdown-item">
                                <div className="media">
                                    <img src="assets/img/user8-128x128.jpg" alt="User Avatar" className="img-size-50 img-circle mr-3" />
                                    <div className="media-body">
                                        <h3 className="dropdown-item-title">
                                            John Pierce
                  <span className="float-right text-sm text-muted"><i className="fas fa-star"></i></span>
                                        </h3>
                                        <p className="text-sm">I got your message bro</p>
                                        <p className="text-sm text-muted"><i className="far fa-clock mr-1"></i> 4 Hours Ago</p>
                                    </div>
                                </div>
                            </a>
                            <div className="dropdown-divider"></div>
                            <a href="#" className="dropdown-item">
                                <div className="media">
                                    <img src="assets/img/user3-128x128.jpg" alt="User Avatar" className="img-size-50 img-circle mr-3" />
                                    <div className="media-body">
                                        <h3 className="dropdown-item-title">
                                            Nora Silvester
                  <span className="float-right text-sm text-warning"><i className="fas fa-star"></i></span>
                                        </h3>
                                        <p className="text-sm">The subject goes here</p>
                                        <p className="text-sm text-muted"><i className="far fa-clock mr-1"></i> 4 Hours Ago</p>
                                    </div>
                                </div>
                            </a>
                            <div className="dropdown-divider"></div>
                            <a href="#" className="dropdown-item dropdown-footer">See All Messages</a>
                        </div>
                    </li> */}
                    {/* <li className="nav-item dropdown">
                        <a href="#" className="nav-link" data-toggle="dropdown">
                            <i className="far fa-bell"></i>
                            <span className="badge badge-warning navbar-badge">15</span>
                        </a>
                        <div className="dropdown-menu dropdown-menu-lg dropdown-menu-right">
                            <span className="dropdown-item dropdown-header">15 Notifications</span>
                            <div className="dropdown-divider"></div>
                            <a href="#" className="dropdown-item">
                                <i className="fas fa-envelope mr-2"></i> 4 new messages
            <span className="float-right text-muted text-sm">3 mins</span>
                            </a>
                            <div className="dropdown-divider"></div>
                            <a href="#" className="dropdown-item">
                                <i className="fas fa-users mr-2"></i> 8 friend requests
            <span className="float-right text-muted text-sm">12 hours</span>
                            </a>
                            <div className="dropdown-divider"></div>
                            <a href="#" className="dropdown-item">
                                <i className="fas fa-file mr-2"></i> 3 new reports
            <span className="float-right text-muted text-sm">2 days</span>
                            </a>
                            <div className="dropdown-divider"></div>
                            <a href="#" className="dropdown-item dropdown-footer">See All Notifications</a>
                        </div>
                    </li> */}
                    <li className="nav-item">
                        <a href="#" className="nav-link" data-widget="fullscreen" role="button">
                            <i className="fas fa-expand-arrows-alt"></i>
                        </a>
                    </li>
                </ul>
            </nav >
        )
    }
}
const mapDispatchToProps = (dispatch) => {
    return {
        changeTab: (tabName) => {
            dispatch({ type: "changeTab", tab: tabName })
        }
    }
}
export default connect(null, mapDispatchToProps)(Header)