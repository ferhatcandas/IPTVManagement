import React, { Component } from 'react'
import ReactHlsPlayer from "react-hls-player";

export default class WatchModal extends Component {
    constructor(props) {
        super(props)
        this.state = {
            url: props.watchUrl,
            id: "modal-watch"
        }
    }
    componentDidUpdate() {
        if (this.props.watchUrl != this.state.url) {
            console.log(this.props.watchUrl)
            this.setState({ url: this.props.watchUrl })
        }
    }
    render() {
        return (
            <div className="modal fade" id={this.state.id} style={{ display: "none" }} aria-modal="true" role="dialog" >
                <div className="modal-dialog">
                    <div className="modal-content bg-primary">
                        <div className="modal-body">
                            <ReactHlsPlayer url={this.state.url} autoPlay={true} controls={true} width="100%" height="100%" />
                        </div>
                    </div>
                </div>
            </div>
        )

    }
}
