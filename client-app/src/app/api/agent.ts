import axios, { AxiosResponse } from 'axios';
import { AuthService } from './auth';
import { User } from 'oidc-client';
import { ISeriesForList, ICategory, ISeriesDetails } from '../models/series';
import { IProfile } from '../models/profile';
import { IArtist } from '../models/artist';

axios.defaults.baseURL = "https://localhost:5101";

var auth = new AuthService();
var refreshing = false;

axios.interceptors.request.use(async (config) => {
    var user: User|null = await auth.getUser();
    if(user) {
        const token = user.access_token;
        config.headers.Authorization = `Bearer ${token}`;
    }
    return config;
}, error => {
    return Promise.reject(error);
})

axios.interceptors.response.use(
    response => response, 
    error => {
        console.log(error);
        if(error.response) {
            var axiosConfig = error.response.config;
            if(error.response.status === 401){
                if(!refreshing) {
                    refreshing = true;
                    return auth.renewToken().then(user => {
                        console.log(user);
                        axios(axiosConfig);
                    })
                }
            }
        }
        return Promise.reject(error);
    })

const responseBody = (response: AxiosResponse) => response.data;

const requests = {
    get: (url: string) => axios.get(url).then(responseBody),
    post: (url: string, body: {}) => axios.post(url, body).then(responseBody),
    put: (url: string, body: {}) => axios.put(url, body).then(responseBody),
    del: (url: string) => axios.delete(url).then(responseBody) 
};

const series = {
    list: (params: URLSearchParams): Promise<AxiosResponse<ISeriesForList[]>> => axios.get("/series", {params: params}),
    get: (id: number): Promise<ISeriesDetails> => axios.get(`/series/${id}`).then(responseBody)
}

const categories = {
    list: (): Promise<ICategory[]> => requests.get("/categories")
}

const artists = {
    list: (params: URLSearchParams): Promise<AxiosResponse<IArtist[]>> => axios.get("/artists", {params: params}), 
}

const profile = {
    get: (userId: string): Promise<IProfile> => requests.get(`/users/${userId}`)
}

export default {
    requests,
    series,
    categories,
    profile,
    artists
}