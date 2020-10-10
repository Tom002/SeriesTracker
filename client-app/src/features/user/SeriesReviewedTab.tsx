import React from "react";
import { Grid } from "semantic-ui-react";
import { ISeriesLikedListItem, ISeriesWatchedListItem, IUserSeriesReview } from "../../app/models/series";
import SeriesLikedCard from "./SeriesLikedCard";
import SeriesWatchedCard from "./SeriesLikedCard";
import UserSeriesReviewCard from "./UserSeriesReviewCard";

export const SeriesReviewedTab: React.FC<{ reviewList: IUserSeriesReview[] }> = 
    (
        {reviewList}) => {
       return (

        <Grid style={{marginLeft: '2em', marginRight: '2em'}}>
            <Grid.Row columns={3} stretched>
            {reviewList.filter(r => r.reviewText && r.reviewTitle).map(review => (
                <Grid.Column>
                    <UserSeriesReviewCard review={review} />
                </Grid.Column>
            ))}
            </Grid.Row>
        </Grid>
       )
}