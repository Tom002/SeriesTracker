import axios, { AxiosResponse } from 'axios';
import { AuthService } from './auth';
import { User } from 'oidc-client';
import { ISeriesForList, ICategory, ISeriesDetails, IEpisode, IUserSeriesReviewInfo, ISeriesWatchedInfo, ISeriesReviewRequest, ISeriesRateRequest, IEpisodeReviewRequest, IUserSeriesReview, ISeriesWatchedListItem, ISeriesLikedListItem, IReview } from '../models/series';
import { IProfile } from '../models/profile';
import { IArtist, IArtistDetails } from '../models/artist';

axios.defaults.baseURL = "http://51.105.152.13/";

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

const series = {
    list: (params: URLSearchParams): Promise<AxiosResponse<ISeriesForList[]>> => 
        axios.get("/series", {params: params}),
    
    get: (id: number): Promise<ISeriesDetails> =>
         axios.get(`/series/${id}`).then(responseBody),
    
    getSeason: (seriesId: number, season: number): Promise<IEpisode[]> => 
        axios.get(`/series/${seriesId}/season/${season}`).then(responseBody)
}

const categories = {
    list: (): Promise<ICategory[]> => axios.get("/series/categories").then(responseBody)
}

const artists = {
    list: (params: URLSearchParams): Promise<AxiosResponse<IArtist[]>> =>
        axios.get("/artist", {params: params}),
    
    get: (artistId: number): Promise<IArtistDetails> =>
        axios.get(`/artist/${artistId}`).then(responseBody)
}

const watching = {
    getSeriesWatchedInfo: (seriesId: number): Promise<ISeriesWatchedInfo> =>
        axios.get(`/watching/series/${seriesId}/myWatching`).then(responseBody),
    
    getSeriesWatched: (userId: string, params?: URLSearchParams): Promise<ISeriesWatchedListItem[]> =>
        axios.get(`/watching/viewers/${userId}/series/watched`, {params: params}).then(responseBody),
    
    getSeriesLiked: (userId: string, params?: URLSearchParams): Promise<ISeriesLikedListItem[]> =>
        axios.get(`/watching/viewers/${userId}/series/liked`, {params: params}).then(responseBody),
        
    watchSeries: (seriesId: number) =>
        axios.post(`/watching/series/${seriesId}/watch`),

    deleteSeriesWatched: (seriesId: number) =>
        axios.delete(`/watching/series/${seriesId}/watch`),
        
    watchEpisode: (episodeId: number) =>
        axios.post(`/watching/episode/${episodeId}/watch`),
    
    deleteEpisodeWatched: (episodeId: number) =>
        axios.delete(`/watching/episode/${episodeId}/watch`),

    likeSeries: (seriesId: number) =>
        axios.post(`/watching/series/${seriesId}/like`),
    
    deleteSeriesLiked: (seriesId: number) =>
        axios.delete(`/watching/series/${seriesId}/like`),
}

const review = {
    getUserReviews: (userId: string, params?: URLSearchParams) : Promise<IUserSeriesReview[]> =>
        axios.get(`/review/users/${userId}`, {params: params}).then(responseBody),

    getMyReview: (seriesId: number): Promise<IUserSeriesReviewInfo> =>
        axios.get(`/review/series/${seriesId}/myReview`).then(responseBody),
    
    getSeriesReviews: (seriesId: number, params?: URLSearchParams): Promise<IReview[]> =>
        axios.get(`/review/series/${seriesId}`, {params: params}).then(responseBody),
    
    reviewSeries: (seriesId: number, data: ISeriesReviewRequest): Promise<IUserSeriesReviewInfo> =>
        axios.put(`/review/series/${seriesId}/review`, data).then(responseBody),
    
    rateSeries: (seriesId: number, data: ISeriesRateRequest): Promise<IUserSeriesReviewInfo> =>
        axios.put(`/review/series/${seriesId}/rate`, data).then(responseBody),

    rateEpisode: (episodeId: number, data: IEpisodeReviewRequest) =>
        axios.put(`/review/episode/${episodeId}`, data)
}

const profile = {
    get: (userId: string): Promise<IProfile> =>
        axios.get(`/users/${userId}`).then(responseBody)
}

export default {
    series,
    categories,
    profile,
    artists,
    watching,
    review
}