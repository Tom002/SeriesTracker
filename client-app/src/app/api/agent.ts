import axios, { AxiosResponse } from 'axios';
import { AuthService } from './auth';
import { User } from 'oidc-client';
import { ISeriesForList, ICategory, ISeriesDetails, IEpisode, ISeriesWatchedList, ISeriesEpisodesWatchedList, IWatchSeriesRequest, IWatchEpisodeRequest, IReviewSeriesRequest, IReviewEpisodeRequest, IUserSeriesReviewInfo } from '../models/series';
import { IProfile } from '../models/profile';
import { IArtist } from '../models/artist';

axios.defaults.baseURL = "http://20.54.142.229/";

var auth = new AuthService();
var refreshing = false;

axios.interceptors.request.use(async (config) => {
    var user: User | null = await auth.getUser();
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
const fullResponse = (response: AxiosResponse) => response;


const series = {
    list: (params: URLSearchParams): Promise<AxiosResponse<ISeriesForList[]>> => axios.get("/series", {params: params}),
    get: (id: number): Promise<ISeriesDetails> => axios.get(`/series/${id}`).then(responseBody),
    getSeason: (seriesId: number, season: number): Promise<IEpisode[]> => 
        axios.get(`/series/${seriesId}/season/${season}`).then(responseBody)
}

const categories = {
    list: (): Promise<ICategory[]> => axios.get("/categories").then(responseBody)
}

const artists = {
    list: (params: URLSearchParams): Promise<AxiosResponse<IArtist[]>> => axios.get("/artists", {params: params}), 
}

const watching = {
    listSeriesWatched:(userId: string): Promise<ISeriesWatchedList> => 
        axios.get(`/watching/${userId}/series/watched`).then(responseBody),
    listEpisodesWatched:(userId: string, seriesId: number): Promise<ISeriesEpisodesWatchedList> => 
        axios.get(`/watching/${userId}/series/${seriesId}/episodes`).then(responseBody),
    watchSeries: (data: IWatchSeriesRequest) => 
        axios.post('/watching/series', data),
    watchEpisode: (data: IWatchEpisodeRequest) =>
        axios.post('/watching/episode', data) 
}

const review = {
    reviewSeries: (data: IReviewSeriesRequest) =>
        axios.post(`/review/series/${data.seriesId}`, data),
    reviewEpisode: (data: IReviewEpisodeRequest) =>
        axios.post('/review/episode/', data),
    
    getUserReview: (userId: string, seriesId: number) : Promise<IUserSeriesReviewInfo> =>
        axios.get(`/review/series/${seriesId}/user/${userId}`).then(responseBody)
}

const profile = {
    get: (userId: string): Promise<IProfile> => axios.get(`/users/${userId}`).then(responseBody)
}

export default {
    series,
    categories,
    profile,
    artists,
    watching,
    review
}