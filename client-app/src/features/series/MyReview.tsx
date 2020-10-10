import React from "react";
import { Link } from "react-router-dom";
import { Button, Card, Comment, Header, Icon } from 'semantic-ui-react'
import { IReview, IUserSeriesReviewInfo } from "../../app/models/series";

const MyReview: React.FC<{review: IUserSeriesReviewInfo}> = (
    {review
}) => {
    return (
        <Card>
            <Card.Content header={review.reviewTitle} />
            <Card.Content description={review.reviewText} />
            <Card.Content extra>
            <Icon name='star' size='big' /> {review.rating}
            </Card.Content>
        </Card>
    )
}
export default MyReview;