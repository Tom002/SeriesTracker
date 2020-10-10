import React from "react";
import { Grid } from "semantic-ui-react";
import { ISeriesWatchedListItem } from "../../app/models/series";
import SeriesWatchedCard from "./SeriesLikedCard";

export const SeriesWatchedTab: React.FC<{ seriesList: ISeriesWatchedListItem[] }> = 
    (
        {seriesList}) => {
       return (

        <Grid style={{marginLeft: '2em', marginRight: '2em'}}>
            <Grid.Row columns={4} stretched>
            {seriesList.map(series => (
                <Grid.Column>
                    <SeriesWatchedCard series={series} />
                </Grid.Column>
            ))}
            </Grid.Row>
        </Grid>
       )     

}