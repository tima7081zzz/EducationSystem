import React from 'react';
import { Card, CardContent, Typography, CardActions, Button } from '@mui/material';
import { Link } from 'react-router-dom';

interface CourseCardProps {
  id: number;
  title: string;
  description: string;
}

const CourseCard: React.FC<CourseCardProps> = ({ id, title, description }) => {
  return (
    <Card>
      <CardContent>
        <Typography variant="h5" component="div">
          {title}
        </Typography>
        <Typography variant="body2" color="text.secondary">
          {description}
        </Typography>
      </CardContent>
      <CardActions>
        <Button size="small" component={Link} to={`/course/${id}`}>
          Open
        </Button>
      </CardActions>
    </Card>
  );
};

export default CourseCard;
