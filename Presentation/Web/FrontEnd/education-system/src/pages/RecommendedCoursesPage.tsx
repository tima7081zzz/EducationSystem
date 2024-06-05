import React, { useEffect, useState } from 'react';
import { Container, Grid, Fab, Menu, MenuItem } from '@mui/material';
import AddIcon from '@mui/icons-material/Add';
import CourseCard from '../components/CourseCard';
import { getRecommendedCourses, UserCourseModel, joinCourse } from '../services/courseService';
import JoinCourseDialog from '../components/JoinCourseDialog';

const RecommendedCoursesPage: React.FC = () => {
  const [courses, setCourses] = useState<UserCourseModel[]>([]);
  const [anchorEl, setAnchorEl] = useState<null | HTMLElement>(null);
  const [joinCourseDialogOpen, setJoinCourseDialogOpen] = useState(false);

  useEffect(() => {
    const fetchCourses = async () => {
      try {
        const data = await getRecommendedCourses();
        setCourses(data.userCourses);
      } catch (error) {
        console.error('Failed to fetch courses:', error);
      }
    };

    fetchCourses();
  }, []);

  const handleMenuClick = (event: React.MouseEvent<HTMLElement>) => {
    setAnchorEl(event.currentTarget);
  };

  const handleMenuClose = () => {
    setAnchorEl(null);
  };

  const handleJoinCourse = async (publicId: string) => {
    try {
      await joinCourse(publicId);
      // Optionally, you can navigate to the course page after joining
      // history.push(`/course/${publicId}`);
      setJoinCourseDialogOpen(false);
      window.location.reload();
    } catch (error) {
      console.error('Failed to join course:', error);
    }  
  };

  return (
    <Container>
      <Grid container spacing={3}>
        {courses.map((course) => (
          <Grid item key={course.id} xs={12} sm={6} md={4} style={{ display: 'flex' }}>
            <CourseCard course={course} />
          </Grid>
        ))}
      </Grid>
      <Fab color="primary" aria-label="add" onClick={handleMenuClick} style={{ position: 'fixed', bottom: 16, right: 16 }}>
        <AddIcon />
      </Fab>
      <Menu anchorEl={anchorEl} open={Boolean(anchorEl)} onClose={handleMenuClose}>
        <MenuItem onClick={() => { setJoinCourseDialogOpen(true); handleMenuClose(); }}>Join course</MenuItem>
      </Menu>
      <JoinCourseDialog
        open={joinCourseDialogOpen}
        onClose={() => setJoinCourseDialogOpen(false)}
        onJoin={handleJoinCourse}
      />
    </Container>
  );
};

export default RecommendedCoursesPage;
