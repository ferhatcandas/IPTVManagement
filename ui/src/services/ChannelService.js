import axios from 'axios';

const baseEndpoint = "http://192.168.1.110:8080/channels";

const RequestURL = (value) => {
    return baseEndpoint + "/" + value;
}
export const ChannelService = ({
    delete: (channelId, callback) => {
        axios.delete(RequestURL(channelId)).then(res => {
            callback(res)
        })
    },
    put: (channel, callback) => {
        axios.put(RequestURL(channel.id), channel).then(res => {
            callback(res)
        })
    },
    putStatus: (channelId, callback) => {
        axios.put(RequestURL(channelId) + "/status", null).then(res => {
            callback(res)
        })
    },
    post: (channel, callback) => {
        axios.post(baseEndpoint, channel).then(res => {
            callback(res)
        })
    },
    get: (channelId, callback) => {
        axios.get(RequestURL(channelId)).then(res => {
            callback(res.data)
        })
    },
    getAll: (callback) => {
        axios.get(baseEndpoint).then(res => {
            callback(res.data)
        })
    }
})