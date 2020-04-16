import { ISeriesForList } from "../../app/models/series";
import React, { useContext, useEffect } from 'react';
import { RouteComponentProps } from "react-router-dom";
import SeriesStore from '../../app/stores/seriesStore';
import { observer } from "mobx-react-lite";

interface SeriesDetailParams {
    id: string;
}

const SeriesDetails: React.FC<RouteComponentProps<SeriesDetailParams>> = ({
    match,
    history
}) =>{
    const seriesStore = useContext(SeriesStore);
    const {getSingleSeries, selectedSeries} = seriesStore;

    useEffect(() => {
        console.log("SeriesDetail");
        if(match.params.id) {
            let id = Number(match.params.id);
            if(isNaN(id)) {

            }
            else {
                getSingleSeries(id).then(() => {
                    console.log(selectedSeries);
                })
            }
        }
    }, [])


    return(
        <h1>Hello</h1>
    )
}



export default observer(SeriesDetails);