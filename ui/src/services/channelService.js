import axios from 'axios';

export default class ChannelService {

    delete(channel, callback) {
        axios.delete(this.RequestURL(channel.id)).then(res => {
            callback(res)
        })
    }
    put(channel, callback) {
        axios.put(this.RequestURL(channel.id), channel).then(res => {
            callback(res)
        })
    }
    post(channel, callback) {
        axios.post(this.endpoint, channel).then(res => {
            callback(res)
        })
    }
    /**
     * 
     * @param {String} channelId 
     * @param {Function} callback 
     */
    get(channelId, callback) {
        axios.get(this.RequestURL(channelId)).then(res => {
            callback(res.data)
        })
    }
    getAll(callback) {
        axios.get(this.endpoint).then(res => {
            callback(res.data)
        })
    }
    RequestURL(value) {
        return this.endpoint + "/" + value;
    }
    constructor() {
        this.endpoint = "http://localhost:5000/channels"
    }
}