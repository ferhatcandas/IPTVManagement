import React, { Component } from 'react';
import ChannelService from '../../services/channelService'

export default class ChannelModal extends Component {
    constructor(props) {
        super(props)
        this.state = {
            id: props.channelId,
            channel: {
                name: "",
                isActive: false,
                logo: "",
                language: "",
                country: "",
                category: "",
                streamLink: "",
                tags: "",
                isFound: false
            }
        }
        this.ChannelApi = new ChannelService();
        this.changeField = this.changeField.bind(this)
        this.updateChannel = this.updateChannel.bind(this)
        this.addNewChannel = this.addNewChannel.bind(this)
    }
    FetchChannel() {
        this.setState({ id: this.props.channelId }, () => {
            this.ChannelApi.get(this.props.channelId, (data) => {
                this.setState({ channel: data })
            })
        })
    }
    changeField(e) {
        const name = e.target.name;
        const value = e.target.value
        let copyObject = this.state;

        copyObject["channel"][name] = value

        this.setState(copyObject)
    }
    updateChannel() {
        let channel = this.state.channel;
        channel.id = this.state.id;
        this.ChannelApi.put(channel, (result) => {
            this.props.onDone();
            document.getElementById("modal-close-button").click()
        })

    }
    addNewChannel() {

    }
    componentDidUpdate() {
        if (this.props.channelId != this.state.id)
            this.FetchChannel()
    }

    render() {
        return (
            <div className="modal fade" id="modal-primary" data={this.props.channelId} style={{ display: 'none' }} aria-modal="true" role="dialog" >
                <div className="modal-dialog">
                    <div className="modal-content bg-primary">
                        <div className="modal-header">
                            <h4 className="modal-title">{(this.state.id != '' ? 'Update Channel' : 'Add New Channel')}</h4>
                            <button type="button" className="close" data-dismiss="modal" aria-label="Close">
                                <span aria-hidden="true">×</span>
                            </button>
                        </div>
                        <div className="modal-body">
                            <div className="form-group">
                                <label htmlFor="channelName">Channel Name</label>
                                <input type="text" className="form-control" name="name" id="channelName" onChange={this.changeField} placeholder="channel name" value={this.state.channel.name} />
                            </div>
                            <div className="form-group row">
                                <div className="col-md-2">
                                    <img src={this.state.channel.logo} width={80} height={70} />
                                </div>
                                <div className="col-md-10">
                                    <label htmlFor="logo">Logo</label>
                                    <input type="text" className="form-control" id="logo" name="logo" onChange={this.changeField} placeholder="logo url" value={this.state.channel.logo} />
                                </div>
                            </div>
                            <div className="form-group">
                                <label htmlFor="language">Language</label>
                                <input type="text" className="form-control" id="language" name="language" onChange={this.changeField} placeholder="Turkish,Arabic,English...." value={this.state.channel.language} />
                            </div>
                            <div className="form-group">
                                <label htmlFor="category">Category</label>
                                <input type="text" className="form-control" id="category" name="category" onChange={this.changeField} placeholder="Haber,Ulusal,Spor" value={this.state.channel.category} />
                            </div>
                            <div className="form-group">
                                <label htmlFor="country">Country</label>
                                <input type="text" className="form-control" id="country" name="country" onChange={this.changeField} placeholder="TR,GB,EG,DZ" value={this.state.channel.country} />
                            </div>
                            <div className="form-group">
                                <label htmlFor="tags">Tags</label>
                                <input type="text" className="form-control" id="tags" name="tags" onChange={this.changeField} placeholder="X,Y,Z" value={this.state.channel.tags} />
                            </div>
                            <div className="form-group">
                                <label htmlFor="streamlink">Stream Link</label>
                                <input type="text" className="form-control" id="streamlink" name="streamlink" onChange={this.changeField} placeholder="m3u8 link" value={this.state.channel.streamLink} />
                            </div>
                            {/* <div className="form-group">
                                    <label htmlFor="channelName">Is Active</label>
                                    <input type="text" className="form-control" id="channelName" placeholder="Channel Name" defaultValue={this.state.channel.name} />
                                </div>
                                <div className="form-group">
                                    <label htmlFor="channelName">Is Found</label>
                                    <input type="text" className="form-control" id="channelName" placeholder="Channel Name" defaultValue={this.state.channel.name} />
                                </div> */}
                        </div>
                        <div className="modal-footer justify-content-between">
                            <button type="button" className="btn btn-outline-light" id="modal-close-button" data-dismiss="modal" >Close</button>
                            <button type="button" className="btn btn-outline-light" onClick={this.props.channelId != '' ? this.updateChannel : this.addNewChannel} >Save changes</button>
                        </div>
                    </div>
                </div>
            </div>
        )
    }
}