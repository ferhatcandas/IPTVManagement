import axios from 'axios';
import config from '../config.json';
export default class IntegrationService {

    delete(id, callback) {
        axios.delete(this.RequestURL(id)).then(res => {
            callback(res)
        })
    }
    put(model, callback) {
        axios.put(this.RequestURL(model.id), model).then(res => {
            callback(res)
        })
    }
    post(model, callback) {
        axios.post(this.endpoint, model).then(res => {
            callback(res)
        })
    }
    get(id, callback) {
        axios.get(this.RequestURL(id)).then(res => {
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
        this.endpoint = config.Api.EndPoint + "/integrations"
    }
}