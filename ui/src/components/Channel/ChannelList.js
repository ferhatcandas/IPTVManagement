import React from 'react';
import ChannelModal from "./ChannelModal";
import ChannelService from "../../services/channelService";

export default class ChannelList extends React.Component {
    constructor(props) {
        super(props)
        this.state = {
            channels: [],
            filtered: [],
            selectedChannelId: ""
        }
        this.channelApi = new ChannelService();
        this.fetchRows = this.fetchRows.bind(this)
    }
    removeChannel(channelId) {
        console.log(channelId);
    }
    editChannel(channelId) {
        this.setState({ selectedChannelId: channelId })
    }

    GenerateRow = (props) => {
        const row = (
            <tr key={props.id} style={{ display: props.show }}>
                <td>{props.channelName}</td>
                <td>
                    <div className="icheck-primary d-inline">
                        <input type="radio" id={"isActive_" + props.id}   disabled={!props.isActive} defaultChecked={props.isActive}></input>
                        <label htmlFor={"isActive_" + props.id}></label>
                    </div>
                </td>
                <td><img src={props.logo} width="40" height="40"></img></td>
                <td>{props.language}</td>
                <td>{props.category}</td>
                <td>{props.country}</td>
                <td><a href={props.streamUrl}>link</a></td>
                <td>
                    <div className="icheck-primary d-inline">
                        <input type="radio" id={"isFound_" + props.id} disabled={!props.isFound} defaultChecked={props.isFound}></input>
                        <label htmlFor={"isFound_" + props.id}></label>
                    </div>
                </td>
                <td><button type="button" onClick={this.editChannel.bind(this, props.id)} data-toggle="modal" data-target="#modal-primary" className="btn btn-warning"><i className="fas fa-pen-square"></i></button></td>
                <td><button type="button" onClick={this.removeChannel.bind(this, props.id)} className="btn btn-danger"><i className="fas fa-eraser"></i></button></td>
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

            let filter = this.state.channels.map((data, index) => {
                if (data.channelName.toLowerCase().includes(value) ||
                    data.language.toLowerCase().includes(value) ||
                    data.category.toLowerCase().includes(value) ||
                    data.country.toLowerCase().includes(value) ||
                    data.streamUrl.toLowerCase().includes(value)) {
                    data.show = 'table-row'
                }
                else {
                    data.show = 'none'
                }
                return data;
            })
            this.setState({ filtered: filter })
        }
        else {
            this.setState({ filtered: this.state.channels })
        }

    }
    fetchRows() {
        this.channelApi.getAll((data) => {
            this.setState({ channels: data, filtered: data, selectedChannelId: '' })
        })

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
                        <th>Stream m3u8</th>
                        <th>Is Found</th>
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
            <div className="col-12">
                <div className="card">
                    <div className="card-header">
                        <h3 className="card-title">TV Channels</h3>

                        <div className="card-tools">
                            <div className="input-group input-group-sm" style={{ width: '150px' }}>
                                <input type="text" name="table_search" className="form-control float-right" onChange={this.filterRows.bind(this)} placeholder="Search" />

                                <div className="input-group-append">
                                    <button type="submit" className="btn btn-default">
                                        <i className="fas fa-search"></i>
                                    </button>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div className="card-body table-responsive p-0">
                        {this.resultTable()}
                    </div>
                </div>
                <ChannelModal channelId={this.state.selectedChannelId} onDone={this.fetchRows} />
            </div>
        )
        return element
    }

}