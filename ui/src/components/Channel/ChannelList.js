import React from 'react';
import ChannelModal from "./ChannelModal";
import ChannelService from "../../services/channelService";
import WatchModal from './WatchModal';

export default class ChannelList extends React.Component {
    constructor(props) {
        super(props)
        this.state = {
            channels: [],
            filtered: [],
            selectedChannelId: "",
            filterText: "",
            watchUrl: ""
        }
        this.channelApi = new ChannelService();
        this.fetchRows = this.fetchRows.bind(this)
        this.removeChannel = this.removeChannel.bind(this);
        this.editChannel = this.editChannel.bind(this);
        this.changeStatus = this.changeStatus.bind(this);
        this.newChannelModal = this.newChannelModal.bind(this);
        this.watch = this.watch.bind(this);
        this.handler = this.handler.bind(this);
    }
    watch(streamUrl) {
        this.setState({ watchUrl: streamUrl })
    }
    removeChannel(channelId) {
        this.channelApi.delete(channelId, () => {
            this.fetchRows();
        })
    }
    changeStatus(channelId) {
        if (channelId)
            this.channelApi.putStatus(channelId, () => {
                this.fetchRows();
            })
    }
    editChannel(channelId) {
        this.setState({ selectedChannelId: channelId })
    }
    newChannelModal() {
        this.setState({ selectedChannelId: null })
    }
    GenerateRow = (props) => {
        const row = (
            <tr key={props.id} style={{ display: props.show }}>
                <td>{props.channelName}</td>
                <td>
                    <div className="icheck-primary d-inline">
                        <input type="radio" id={"isActive_" + props.id} onClick={this.changeStatus.bind(this, props.id)} disabled={!props.editable} onChange={this.changeStatus.bind(this, null)} checked={props.isActive}></input>
                        <label htmlFor={"isActive_" + props.id}></label>
                    </div>
                </td>
                <td><img src={props.logo} width={40} height={40} alt="" /></td>
                <td>{props.language}</td>
                <td>{props.category}</td>
                <td>{props.country}</td>
                <td>
                    <div className="icheck-primary d-inline">
                        <input type="radio" id={"isFound_" + props.id} disabled={!props.isFound} defaultChecked={props.isFound}></input>
                        <label htmlFor={"isFound_" + props.id}></label>
                    </div>
                </td>

                <td><button type="button" onClick={this.watch.bind(this, props.streamUrl)} data-toggle="modal" data-target="#modal-watch" className="btn btn-warning"><i className="fas fa-tv"></i></button></td>
                <td>{props.editable ? <button type="button" onClick={this.editChannel.bind(this, props.id)} data-toggle="modal" data-target="#modal-primary" className="btn btn-warning"><i className="fas fa-pen-square"></i></button> : null}</td>
                <td>{props.editable ? <button type="button" onClick={this.removeChannel.bind(this, props.id)} className="btn btn-danger"><i className="fas fa-eraser"></i></button> : null}</td>
            </tr>
        )
        return row;
    };

    GetTable() {
        var rows = [];
        for (let index = 0; index < this.state.filtered.length; index++) {
            const element = this.GenerateRow(this.state.filtered[index]);
            rows.push(element);
        }
        return rows
    }
    filterRows = (event) => {
        if (event.target.value) {
            let value = event.target.value.toLowerCase();
            this.filterFunction(value);
        }
        else {
            this.setState({ filtered: this.state.channels, filterText: "" })
        }
    }
    filterFunction = (value) => {
        let filter = this.state.channels.map((data, index) => {
            if (data.channelName?.toLowerCase().includes(value) ||
                data.language?.toLowerCase().includes(value) ||
                data.category?.toLowerCase().includes(value) ||
                data.country?.toLowerCase().includes(value) ||
                data.streamUrl?.toLowerCase().includes(value)) {
                data.show = 'table-row'
            }
            else {
                data.show = 'none'
            }
            return data;
        })
        this.setState({ filtered: filter, filterText: value })
    }
    fetchRows() {
        this.channelApi.getAll((data) => {
            this.setState({ channels: data, selectedChannelId: '' }, () => {
                this.filterFunction(this.state.filterText)
            })
        })

    }
    handler() {
        if (this.state.watchUrl != "")
            this.setState({ watchUrl: "" })
    }

    componentDidMount() {
        this.fetchRows();
    }
    resultTable() {
        const element = (
            <table className="table table-hover text-nowrap">
                <thead>
                    <tr>
                        <th>Channel Name</th>
                        <th>Is Active</th>
                        <th>Logo</th>
                        <th>Language</th>
                        <th>Category</th>
                        <th>Country</th>
                        <th>Is Found</th>
                        <th>Stream m3u8</th>
                        <th>Edit</th>
                        <th>Remove</th>
                    </tr>
                </thead>
                <tbody>
                    {this.GetTable()}
                </tbody>
            </table>
        )
        return element;


    }
    render() {
        const element = (
            <div className="col-12" style={{ paddingLeft: "0px", paddingRight: "0px" }} onClick={this.handler.bind(this)}>
                <div className="card">
                    <div className="card-header">
                        <h3 className="card-title">TV Channels</h3>
                        <div className="container-fluid">
                            <div className="row">
                                <div className="col-8">
                                    <button type="button" className="btn btn-primary float-right" data-toggle="modal" data-target="#modal-primary" onClick={this.newChannelModal.bind(this)} ><i className="fa fa-plus"></i></button>
                                </div>
                                <div className="col-4">
                                    <div className="card-tools">
                                        <div className="input-group input-group-sm">
                                            <input type="text" name="table_search" className="form-control float-right" onChange={this.filterRows.bind(this)} placeholder="Search" />
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div className="card-body table-responsive p-0">
                        {this.resultTable()}
                    </div>
                </div>
                <ChannelModal channelId={this.state.selectedChannelId} onDone={this.fetchRows} />
                <WatchModal watchUrl={this.state.watchUrl} onClose={this.watch} />
            </div>
        )
        return element
    }

}