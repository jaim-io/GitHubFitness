interface Exercise {
  id: string;
  name: string;
  description: string;
  creatorId: string;
  image: string;
  video: string;
  muscleGroupIds: string[];
  muscleIds: string[];
  createdDateTime: Date;
  updatedDateTime: Date;
}

export default Exercise;
