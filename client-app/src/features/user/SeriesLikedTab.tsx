import React from "react";
import { Grid } from "semantic-ui-react";
import { ISeriesLikedListItem, ISeriesWatchedListItem } from "../../app/models/series";
import SeriesLikedCard from "./SeriesLikedCard";
import SeriesWatchedCard from "./SeriesLikedCard";

export const SeriesLikedTab: React.FC<{ seriesList: ISeriesLikedListItem[] }> = 
    (
        {seriesList}) => {
       return (
        <Grid columns={4} container doubling stackable>
            {seriesList.map(series => (
                <Grid.Column>
                    <SeriesLikedCard series={series} />
                </Grid.Column>
            ))}
        </Grid>
       )     

}