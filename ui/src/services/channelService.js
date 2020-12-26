import axios from 'axios';
import config from '../config.json';
export default class ChannelService {

    delete(channelId, callback) {
        axios.delete(this.RequestURL(channelId)).then(res => {
            callback(res)
        })
    }
    put(channel, callback) {
        axios.put(this.RequestURL(channel.id), channel).then(res => {
            callback(res)
        })
    }
    putStatus(channelId, callback) {
        axios.put(this.RequestURL(channelId) + "/status",null).then(res => {
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
        this.endpoint = config.Api.EndPoint + "/channels"
    }
}