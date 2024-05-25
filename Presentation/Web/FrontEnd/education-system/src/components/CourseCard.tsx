import React from 'react';
import { Card, CardContent, Typography, Button, CardActions } from '@mui/material';
import { Link } from 'react-router-dom';
import { UserCourseModel } from '../services/courseService';

interface CourseCardProps {
  course: UserCourseModel;
}

const CourseCard: React.FC<CourseCardProps> = ({ course }) => {
  return (
    <Card style={{ width: '18.75rem', height: '11.375rem', display: 'flex', flexDirection: 'column' }}>
      <CardContent style={{ flexGrow: 1 }}>
        <div>
          <Typography variant="h5" component="div" gutterBottom>
            {course.name}
          </Typography>
          <Typography variant="body2" color="textSecondary" style={{ minHeight: '50px' }}>
            {course.description}
          </Typography>
        </div>
        <div>
          <Typography variant="body2" color="textSecondary">
            {course.creatorUserFullName}
          </Typography>
        </div>
      </CardContent>
      <CardActions>
        <Button size="small" component={Link} to={`/course/${course.id}`}>
          Open
        </Button>
      </CardActions>
    </Card>
  );
};

export default CourseCard;
