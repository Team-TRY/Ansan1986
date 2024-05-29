namespace InsaneSystems.RoadNavigator
{
	public class PointsDistance
	{
		public int pointAId, pointBId;
		public float distance;

		public bool IsSimilar(int pointAId, int pointBId)
		{
			return (this.pointAId == pointAId && this.pointBId == pointBId) || (this.pointAId == pointBId && this.pointBId == pointAId);
		}
	}
}