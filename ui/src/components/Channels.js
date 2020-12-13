import React from 'react';

class ChannelsComponent extends React.Component
{
    constructor(props){
        super(props)
        this.state = {channels:[],filtered:[]}
    }
    removeChannel(channelId) {
        
        console.log(channelId);
    }
    editChannel (channelId)  {
        console.log(channelId);
    }
    GenerateRow = (props) =>{
            const row = (
                        <tr key={props.id}>
                            {/* <th scope="row">{props.id}</th> */}
                            <td>{props.channelName}</td>
                            <td><input type="checkbox" disabled checked={props.isActive}></input></td>
                            <td><img src={props.logo} width="40" height="40"></img></td>
                            <td>{props.language}</td>
                            <td>{props.category}</td>
                            <td>{props.country}</td>
                            <td><a href={props.streamUrl}>link</a></td>
                            <td><input type="checkbox" disabled checked={props.isFound}></input></td>
                            <td><button type="button" onClick={this.editChannel.bind(this,props.id)} className="btn btn-warning">Edit</button></td>
                            <td><button type="button" onClick={this.removeChannel.bind(this,props.id)} className="btn btn-danger">Remove</button></td>
                        </tr>
            )
            return row;
        };
  
    GetTable(){
        var rows = [];
        for (let index = 0; index < this.state.filtered.length; index++) {
            const element =  this.GenerateRow(this.state.filtered[index]);
            rows.push(element);
        }
        return rows
    }
    filterRows = (event) => {
        if(event.target.value) {
            let value = event.target.value.toLowerCase();
            let filter = this.state.channels.filter(x=>
                    x.channelName.toLowerCase().includes(value) || 
                    x.language.toLowerCase().includes(value) ||
                    x.category.toLowerCase().includes(value) || 
                    x.country.toLowerCase().includes(value) ||
                    x.streamUrl.toLowerCase().includes(value)
                );
            this.setState({filtered:filter})
        }
        else
        {
            this.setState({filtered:this.state.channels})
        }
        
    }
    fetchRows(){
        fetch("http://localhost:5000/channels").then(res=>res.json()).then((result) => {
             this.setState({channels:result,filtered:result})
        })
    }
    componentDidMount()
    {
        this.fetchRows();
    }
   

    //#region UI
    searchInput() {
        const element = (
            <div className="form-group row">
                <label htmlFor="searchInput" className="col-md-2 col-form-label">Search :</label>
                <div className="col-md-10">
                    <input type="text"  onChange={this.filterRows.bind(this)} placeholder="channel name, language, category, streamlink .." className="form-control" id="searchInput"></input>
                </div>
            </div>
            )
        return element;
    }
    resultTable() {
        const element =(
            <table className="table table-striped">
            <thead>
              <tr>
                {/* <th scope="col">#Id</th> */}
                <th scope="col">Channel Name</th>
                <th scope="col">Is Active</th>
                <th scope="col">Logo Url</th>
                <th scope="col">Language</th>
                <th scope="col">Category</th>
                <th scope="col">Country</th>
                <th scope="col">Stream Link</th>
                <th scope="col">Is Found</th>
                <th scope="col">Edit</th>
                <th scope="col">Remove</th>
              </tr>
            </thead>
            <tbody>
                {this.GetTable()}
            </tbody>
          </table>
        )
        return element;
    }
    //#endregion
    render(){
        const element = (
            <div>
                <div className="col-md-6">
                    {this.searchInput()}
                </div>
                <div className="col-md-6"></div>
                <div className="col-md-12">
                    {this.resultTable()}
                </div>
           </div>
        )
        return element
    }
   
}
export default ChannelsComponent