import React from "react";
import { Link } from "react-router-dom";
import { Button, Card, Comment, Header, Icon } from 'semantic-ui-react'
import { IReview } from "../../app/models/series";

const SeriesReview: React.FC<{review: IReview}> = (
    {review
}) => {
    return (
        <Card>
            <Card.Content header={review.reviewTitle} />
            <Card.Content description={review.reviewText} />
            <Card.Content extra>
            <Icon name='user' size='big'/> 
            <Button as={Link}
                    to={`/users/${review.reviewerId}`}
                    content={review.reviewerName} />
            <Icon name='star' size='big' /> {review.rating}
            </Card.Content>
        </Card>
    )
}
export default SeriesReview;